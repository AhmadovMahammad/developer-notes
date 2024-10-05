namespace Chapter4
{
    public class Note
    {
        private int _value;

        public Note(int Value)
        {
            _value = Value;
        }

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Note note && Value == note.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, Value);
        }

        public static implicit operator double(Note note)
        {
            return 440 * Math.Pow(2, (double)note.Value / 12);
        }

        public static explicit operator Note(double frequency)
        {
            return new Note((int)(0.5 + 12 * (Math.Log(frequency / 440) / Math.Log(2))));
        }

        public static Note operator +(Note previousNote, int value)
        {
            return new Note(previousNote._value + value);
        }

        public static bool operator ==(Note previousNote, int value)
        {
            return previousNote.Value == value;
        }

        public static bool operator !=(Note previousNote, int value)
        {
            return previousNote.Value != value;
        }

        public static bool operator <(Note previousNote, int value)
        {
            return previousNote.Value < value;
        }

        public static bool operator >(Note previousNote, int value)
        {
            return previousNote.Value > value;
        }

        public override string ToString()
        {
            string[] noteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
            int noteIndex = (_value + 9) % 12;
            int octave = (_value + 9) / 12 + 4;
            return $"{noteNames[noteIndex]}{octave}";
        }
    }
}