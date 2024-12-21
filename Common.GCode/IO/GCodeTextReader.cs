//-----------------------------------------------------------------------
// <copyright file="GCodeTextReader.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Common.GCode.IO;

using System.Text;

/// <summary>
/// Structured text reader for GCode/RS274/NGC files.
/// </summary>
public class GCodeTextReader : GCodeReader
{
    private readonly TextReader inner;
    private GCodeTokenType tokenType;
    private decimal value;
    private GCodeValueType valueType;
    private bool isBlockDeleteLine;
    private int lineNumber = -1;
    private Code code;
    private string? comment;

    /// <summary>
    /// Initializes a new instance of the <see cref="GCodeTextReader"/> class.
    /// </summary>
    /// <param name="inner">The inner reader.</param>
    /// <param name="settings">Optional settings used for the reader.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inner"/> is <see langword="null"/>.
    /// </exception>
    public GCodeTextReader(TextReader inner, GCodeReaderSettings? settings = null)
    : base(settings)
    {
        this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    private enum ReaderState
    {
        None = 0,
        LineStart,
    }

    /// <inheritdoc/>
    public override GCodeTokenType TokenType => this.tokenType;

    /// <inheritdoc/>
    public override decimal Value => this.value;

    /// <inheritdoc/>
    public override GCodeValueType ValueType => this.valueType;

    /// <inheritdoc/>
    public override bool IsBlockDeleteLine => this.isBlockDeleteLine;

    /// <inheritdoc/>
    public override int LineNumber => this.lineNumber;

    /// <inheritdoc/>
    public override Code Code => this.code;

    /// <inheritdoc/>
    public override string? Comment => this.comment;

    private bool CloseInput => this.Settings?.CloseInput ?? true;

    private ReaderState State { get; set; }

    /// <inheritdoc/>
    public override bool Read()
    {
        switch (this.TokenType)
        {
            case GCodeTokenType.None:
            case GCodeTokenType.FileEnd:
                return this.ReadStartFile();
            case GCodeTokenType.FileStart:
            case GCodeTokenType.LineEnd:
                this.ReadLineStart();
                break;
            case GCodeTokenType.LineStart:
            case GCodeTokenType.CommentOrMessage:
            case GCodeTokenType.WordEnd:
                return this.ReadWordStartOrComment();
            case GCodeTokenType.WordStart:
                return this.ReadValue();
            case GCodeTokenType.Value:
                this.ReadWordEnd();
                break;
        }

        return true;
    }

    /// <inheritdoc/>
    protected override ValueTask DisposeAsyncCore()
    {
        if (this.CloseInput)
        {
            this.inner.Dispose();
        }

        return default;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && this.CloseInput)
        {
            this.inner.Dispose();
        }
    }

    private void SkipWhiteSpace()
    {
        int ch = this.inner.Peek();
        while (ch == ' ' || ch == '\t')
        {
            this.inner.Read();
            ch = this.inner.Peek();
        }
    }

    private bool ReadStartFile()
    {
        string? line = this.inner.ReadLine();
        while (line != null && !string.Equals("%", line.Trim(), StringComparison.Ordinal))
        {
            line = this.inner.ReadLine();
        }

        if (line == null)
        {
            return false;
        }

        this.State = ReaderState.LineStart;
        this.tokenType = GCodeTokenType.FileStart;
        return true;
    }

    private void ReadLineStart()
    {
        this.tokenType = GCodeTokenType.LineStart;
        this.lineNumber = -1;
        this.SkipWhiteSpace();
        int ch = this.inner.Peek();
        if (ch == '/')
        {
            this.inner.Read();
            this.isBlockDeleteLine = true;
            this.SkipWhiteSpace();
            ch = this.inner.Peek();
        }

        if (ch == 'N' || ch == 'n')
        {
            this.inner.Read();
            this.lineNumber = this.ReadLineNumber();
        }
    }

    private bool ReadWordStartOrComment()
    {
        this.SkipWhiteSpace();
        int ch = this.inner.Peek();
        if (ch <= 0)
        {
            return false;
        }

        if (ch == '\r' || ch == '\n')
        {
            this.inner.Read();
            if (ch == '\r')
            {
                ch = this.inner.Peek();
                if (ch != '\n')
                {
                    throw new GCodeTextReaderException("Expected newline after carriage return.");
                }

                this.inner.Read();
            }

            this.tokenType = GCodeTokenType.LineEnd;
        }
        else if (ch == '(')
        {
            this.inner.Read();
            StringBuilder sb = new();
            ch = this.inner.Peek();
            while (ch > 0 && ch != ')')
            {
                if (ch == '\r' || ch == '\n')
                {
                    throw new GCodeTextReaderException("Comment not closed before end of line.");
                }

                sb.Append((char)ch);
                this.inner.Read();
                ch = this.inner.Peek();
            }

            if (ch <= 0)
            {
                throw new GCodeTextReaderException("Comment not closed before end of file.");
            }

            this.inner.Read();
            this.tokenType = GCodeTokenType.CommentOrMessage;
            this.comment = sb.ToString();
        }
        else if (ch == ';')
        {
            this.inner.Read();
            this.comment = this.inner.ReadLine() ?? string.Empty;
            this.tokenType = GCodeTokenType.CommentOrMessage;
        }
        else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
        {
            this.tokenType = GCodeTokenType.WordStart;
            this.code = (Code)char.ToUpperInvariant((char)ch);
            this.inner.Read();
        }
        else
        {
            throw new GCodeTextReaderException($"Unexpected character '{(char)ch}' in input.");
        }

        return true;
    }

    private bool ReadValue()
    {
        this.tokenType = GCodeTokenType.Value;
        this.SkipWhiteSpace();
        int ch = this.inner.Peek();
        if (ch <= 0)
        {
            return false;
        }

        bool neg = false;
        if (ch == '+' || ch == '-')
        {
            neg = ch == '-';
            this.inner.Read();
            ch = this.inner.Peek();
        }

        int intPart = 0;
        decimal fracPart = 0M;
        bool allowTrailingDecimalPoint = false;
        if (ch != '.')
        {
            intPart = this.InnerReadInt32();
            allowTrailingDecimalPoint = true;
            ch = this.inner.Peek();
        }

        if (ch == '.')
        {
            this.inner.Read();
            ch = this.inner.Peek();
            if (ch <= '0' || ch >= '9')
            {
                if (!allowTrailingDecimalPoint)
                {
                    throw new GCodeTextReaderException("Expected digit.");
                }

                this.valueType = GCodeValueType.Integer;
                this.value = neg ? -intPart : intPart;
                return true;
            }

            decimal power = 0.1M;
            while (ch >= '0' && ch <= '9')
            {
                int digit = ch - '0';
                fracPart += power * digit;
                power *= 0.1M;
                this.inner.Read();
                ch = this.inner.Peek();
            }

            this.valueType = GCodeValueType.Decimal;
            this.value = neg ? -(intPart + fracPart) : (intPart + fracPart);
        }
        else
        {
            this.value = neg ? -intPart : intPart;
            this.valueType = GCodeValueType.Integer;
        }

        return true;
    }

    private void ReadWordEnd()
    {
        this.SkipWhiteSpace();
        this.tokenType = GCodeTokenType.WordEnd;
    }

    private int ReadLineNumber()
    {
        this.SkipWhiteSpace();

        // TODO: Enforce maximum line number.
        return this.InnerReadInt32();
    }

    private int InnerReadInt32()
    {
        int ch = this.inner.Read();
        if (ch < '0' || ch > '9')
        {
            throw new GCodeTextReaderException("Expected digit.");
        }

        int value = ch - '0';
        ch = this.inner.Peek();
        while (ch >= '0' && ch <= '9')
        {
            value *= 10;
            value += ch - '0';
            this.inner.Read();
            ch = this.inner.Peek();
        }

        return value;
    }
}