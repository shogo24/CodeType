using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TypingManager : MonoBehaviour
{
    public TMP_Text codeText;

    string code;
    int currentIndex = 0;
    bool hadMistake = false;
    Color[] charColors;

    Color defaultColor = new Color(0.72f, 0.75f, 0.80f);
    Color completedColor = new Color(0.45f, 0.65f, 0.85f);
    Color mistakeColor = new Color(0.92f, 0.48f, 0.50f);
    Color caretColor = new Color(0.88f, 0.91f, 0.98f);

    int maxVisibleLines = 10;
    int firstVisibleLine = 0;

    void Start()
    {
        CodeGenerator generator = GetComponent<CodeGenerator>();
        code = generator.GenerateCode();

        charColors = new Color[code.Length];
        for (int i = 0; i < charColors.Length; i++)
            charColors[i] = defaultColor;

        UpdateVisuals();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (currentIndex >= code.Length - 1)
        {
            GenerateNewCode();
            return;
        }

        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                char typed = KeyToChar(key);
                HandleInput(typed);
                break;
            }
        }
    }

    void HandleInput(char typed)
    {
        if (typed == '\0') return;
        if (currentIndex >= code.Length) return;

        if (typed == '\t')
        {
            const int tabWidth = 4;
            int consumed = 0;
            while (currentIndex < code.Length && (code[currentIndex] == ' ' || code[currentIndex] == '·') && consumed < tabWidth)
            {
                charColors[currentIndex] = hadMistake ? mistakeColor : completedColor;
                currentIndex++;
                consumed++;
                UpdateScroll();
            }

            if (consumed == 0)
            {
                hadMistake = true;
                if (currentIndex < code.Length)
                    charColors[currentIndex] = mistakeColor;
            }
            else
            {
                hadMistake = false;
            }

            UpdateVisuals();
            return;
        }

        char expected = code[currentIndex];

        if (IsMatch(typed, expected))
        {
            charColors[currentIndex] = hadMistake ? mistakeColor : completedColor;
            currentIndex++;
            hadMistake = false;

            UpdateScroll();

            if (expected == '\n')
            {
                while (currentIndex < code.Length)
                {
                    char c = code[currentIndex];
                    if (c == ' ' || c == '\t' || c == '·')
                    {
                        charColors[currentIndex] = completedColor;
                        currentIndex++;
                        UpdateScroll();
                        continue;
                    }
                    break;
                }
            }
        }
        else
        {
            hadMistake = true;
            charColors[currentIndex] = mistakeColor;
        }

        UpdateVisuals();
    }

    bool IsMatch(char typed, char expected)
    {
        if (expected == '·')
            return typed == ' ' || typed == '·';
        if (typed == '·')
            typed = ' ';
        return typed == expected;
    }

    char KeyToChar(KeyControl key)
    {
        if (key == null) return '\0';

        if (key.keyCode == Key.Space) return ' ';
        if (key.keyCode == Key.Enter) return '\n';
        if (key.keyCode == Key.Tab) return '\t';

        var kc = key.keyCode;

        bool shift = Keyboard.current?.shiftKey?.isPressed == true;
        bool caps = Keyboard.current?.capsLockKey?.isPressed == true;

        if (kc >= Key.A && kc <= Key.Z)
        {
            int offset = kc - Key.A;
            char c = (char)('a' + offset);
            if (shift ^ caps) c = char.ToUpper(c);
            return c;
        }

        Key[] digits = new Key[]
        {
            Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
            Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9
        };

        for (int i = 0; i < digits.Length; i++)
        {
            if (kc == digits[i])
            {
                if (shift)
                {
                    string symbols = ")!@#$%^&*(";
                    return symbols[i];
                }
                return (char)('0' + i);
            }
        }

        switch (kc)
        {
            case Key.Minus: return shift ? '_' : '-';
            case Key.Equals: return shift ? '+' : '=';
            case Key.Comma: return shift ? '<' : ',';
            case Key.Period: return shift ? '>' : '.';
            case Key.Slash: return shift ? '?' : '/';
            case Key.Semicolon: return shift ? ':' : ';';
            case Key.Quote: return shift ? '"' : '\'';
            case Key.LeftBracket: return shift ? '{' : '[';
            case Key.RightBracket: return shift ? '}' : ']';
            case Key.Backslash: return shift ? '|' : '\\';
            case Key.Backquote: return shift ? '~' : '`';
        }

        return '\0';
    }

    void UpdateVisuals()
    {
        StringBuilder sb = new StringBuilder();

        int currentLine = 0;

        for (int i = 0; i < code.Length; i++)
        {
            if (currentLine < firstVisibleLine)
            {
                if (code[i] == '\n')
                    currentLine++;
                continue;
            }

            if (currentLine >= firstVisibleLine + maxVisibleLines)
                break;

            Color color = (i == currentIndex) ? caretColor : charColors[i];

            sb.Append("<color=#");
            sb.Append(ColorUtility.ToHtmlStringRGB(color));
            sb.Append(">");

            sb.Append(code[i]);
            sb.Append("</color>");

            if (code[i] == '\n')
                currentLine++;
        }

        codeText.text = sb.ToString();
    }

    int GetLineFromIndex(int index)
    {
        int line = 0;
        for (int i = 0; i < index && i < code.Length; i++)
        {
            if (code[i] == '\n')
                line++;
        }
        return line;
    }

    void UpdateScroll()
    {
        int caretLine = GetLineFromIndex(currentIndex);

        int scrollMargin = 2;

        if (caretLine >= firstVisibleLine + maxVisibleLines - scrollMargin)
        {
            firstVisibleLine = caretLine - maxVisibleLines + 1 + scrollMargin;

            if (firstVisibleLine < 0)
                firstVisibleLine = 0;
        }
    }


    void GenerateNewCode()
    {
        CodeGenerator generator = GetComponent<CodeGenerator>();
        if (generator == null) return;

        code = generator.GenerateCode();
        currentIndex = 0;
        hadMistake = false;
        firstVisibleLine = 0;

        charColors = new Color[code.Length];
        for (int i = 0; i < charColors.Length; i++)
            charColors[i] = defaultColor;

        UpdateVisuals();
    }
}
