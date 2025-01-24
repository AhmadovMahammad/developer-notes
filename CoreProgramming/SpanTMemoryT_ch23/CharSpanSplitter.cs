namespace SpanTMemoryT_ch23;
public ref struct CharSpanSplitter
{
    private readonly Span<char> _input;

    public CharSpanSplitter(Span<char> input)
    {
        _input = input;
    }

    public readonly CharEnumerator GetEnumerator() => new CharEnumerator(_input);
}

public ref struct CharEnumerator
{
    private readonly ReadOnlySpan<char> _input;
    private int _wordPosition;

    public ReadOnlySpan<char> Current { get; private set; }

    public CharEnumerator(ReadOnlySpan<char> input)
    {
        _input = input;
        _wordPosition = 0;
        Current = default;
    }

    public bool MoveNext()
    {
        for (int i = _wordPosition; i <= _input.Length; i++)
        {
            if (i == _input.Length || char.IsWhiteSpace(_input[i]))
            {
                Current = _input[_wordPosition..i];
                _wordPosition = i + 1;

                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        _wordPosition = 0;
    }
}