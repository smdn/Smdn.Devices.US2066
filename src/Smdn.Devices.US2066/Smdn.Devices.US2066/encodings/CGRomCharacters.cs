// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;

namespace Smdn.Devices.US2066 {
  internal class CGRomCharacters {
    internal static readonly IComparer<(char, byte)> CharacterMapEntryComparer =
      Comparer<(char, byte)>.Create(static ((char ch, byte by) x, (char ch, byte by) y) => Comparer<char>.Default.Compare(x.ch, y.ch));

    private static (char, byte)[] GenerateSortedCharacterMap(char[][] map)
    {
      var sortedMap = new (char, byte)[0x100];
      var index = 0;

      for (var hi = 0x00; hi <= 0x0F; hi++) {
        for (var lo = 0x00; lo <= 0x0F; lo++) {
          var byteForCharacter = (byte)((hi << 4) | lo);
          if (map[lo][hi] == c_undef)
            // \uF8XX: undefined characters
            sortedMap[index++] = ((char)(c_undef | byteForCharacter), byteForCharacter);
          else if (map[lo][hi] == c_unmap)
            // \uE2XX: unmapped/unmappable characters
            sortedMap[index++] = ((char)(c_unmap | byteForCharacter), byteForCharacter);
          else
            sortedMap[index++] = (map[lo][hi], byteForCharacter);
        }
      }

      Array.Sort(sortedMap, CharacterMapEntryComparer);

      return sortedMap;
    }

    private const char c_undef = '\uF800';
    private const char c_unmap = '\uE200';

    internal static bool IsUndefined(char ch) => c_undef <= ch && ch <= (char)(c_undef + 0xFF);
    internal static bool IsUnmapped(char ch) => c_unmap <= ch && ch <= (char)(c_unmap + 0xFF);

    internal static readonly (char, byte)[] CharacterMapRomA = GenerateSortedCharacterMap(
      new char[][] {
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
        new[] /* 0x_0 */ {  c_undef,      '▶',      ' ',      '0',      '¡',      'P',      '¿',      'p',      '⁰',      '♪',      '@',  c_unmap,      'Γ', /*!*/'⅟',      '￬',      'Č' },
        new[] /* 0x_1 */ {  c_undef,      '◀',      '!',      '1',      'A',      'Q',      'a',      'q',      '¹',      '♫',      '£',      '¢',      'Λ', /*!*/'⅘',      '￩',      'Ě' },
        new[] /* 0x_2 */ {  c_undef,      '⏫',      '"',      '2',      'B',      'R',      'b',      'r',      '²',  c_unmap,      '$',      'Φ',      'Π', /*!*/'⅗',      'Á',      'Ř' },
        new[] /* 0x_3 */ {  c_undef,      '⏬',      '#',      '3',      'C',      'S',      'c',      's',      '³',      '♥',      '¥',      'τ',      'Υ', /*!*/'⅖',      'Í',      'Š' },
        new[] /* 0x_4 */ {  c_undef,      '⏪',      '¤',      '4',      'D',      'T',      'd',      't',      '⁴',  c_unmap,      'è',      'λ',      '_', /*!*/'⅕',      'Ó',      'Ž' },
        new[] /* 0x_5 */ {  c_undef,      '⏩',      '%',      '5',      'E',      'U',      'e',      'u',      '⁵',  c_unmap,      'é',      'Ω',      'È',      'ƒ',      'Ú',      'č' },
        new[] /* 0x_6 */ {  c_undef,      '↖',      '&',      '6',      'F',      'V',      'f',      'v',      '⁶',      '｢',      'ù',      'π',      'Ê',      '█',      'Ý',      'ě' },
        new[] /* 0x_7 */ {  c_undef,      '↗',     '\'',      '7',      'G',      'W',      'g',      'w',      '⁷',      '｣',      'ì',      'ψ',      'ê',      '▉',      'á',      'ř' },
        new[] /* 0x_8 */ {  c_undef,      '↙',      '(',      '8',      'H',      'X',      'h',      'x',      '⁸',      '“',      'ò',      'Σ',      'ç',      '▋',      'í',      'š' },
        new[] /* 0x_9 */ {  c_undef,      '↘',      ')',      '9',      'I',      'Y',      'i',      'y',      '⁹',      '”',      'Ç',      'Θ',      'ğ',      '▍',      'ó',      'ž' },
        new[] /* 0x_A */ {  c_undef,      '▲',      '*',      ':',      'J',      'Z',      'j',      'z',      '½',      '(',  c_unmap,      'Ξ',      'Ş',      '▏',      'ú',      '[' },
        new[] /* 0x_B */ {  c_undef,      '▼',      '+',      ';',      'K',      'Ä',      'k',      'ä',      '¼',      ')',      'Ø',      '●',      'ş', /*!*/'㌮',      'ý',     '\\' },
        new[] /* 0x_C */ {  c_undef,      '↵',      ',',      '<',      'L',      'Ö',      'l',      'ö',      '±',      'α',      'ø',      'Æ',      'İ',  c_unmap,      'Ô',      ']' },
        new[] /* 0x_D */ {  c_undef,      '^',      '-',      '=',      'M',      'Ñ',      'm',      'ñ',      '≥',      'ε',  c_unmap,      'æ',      'ı',      '･',      'ô',      '{' },
        new[] /* 0x_E */ {  c_undef,      'ˇ',      '.',      '>',      'N',      'Ü',      'n',      'ü',      '≤',      'δ',      'Ȧ',      'β',  c_unmap,      '￪',      'Ů',      '¦' },
        new[] /* 0x_F */ {  c_undef,  c_unmap,      '/',      '?',      'O',  c_unmap,      'o',      'à',      'μ',  c_unmap,      'ȧ',      'É',  c_unmap,      '￫',      'ů',      '}' },
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
      }
    );

    internal static readonly (char, byte)[] CharacterMapRomB = GenerateSortedCharacterMap(
      new char[][] {
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
        new[] /* 0x_0 */ {  c_undef,      '▶',      ' ',      '0',      '@',      'P',      '`',      'p',      'А',      'Р',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_1 */ {  c_undef,      '◀',      '!',      '1',      'A',      'Q',      'a',      'q',      'Б',      'С',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_2 */ {  c_undef,      '⏫',      '"',      '2',      'B',      'R',      'b',      'r',      'В',      'Т',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_3 */ {  c_undef,      '⏬',      '#',      '3',      'C',      'S',      'c',      's',      'Г',      'У',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_4 */ {  c_undef,      '£',      '$',      '4',      'D',      'T',      'd',      't',      'Д',      'Ф',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_5 */ {  c_undef,      '¿',      '%',      '5',      'E',      'U',      'e',      'u',      'Е',      'Х',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_6 */ {  c_undef,      '¡',      '&',      '6',      'F',      'V',      'f',      'v',      'Ж',      'Ц',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_7 */ {  c_undef,      '♪',     '\'',      '7',      'G',      'W',      'g',      'w',      'З',      'Ч',  c_unmap,  c_unmap,  c_unmap,      '×',  c_unmap,      '÷' },
        new[] /* 0x_8 */ {  c_undef,  c_unmap,      '(',      '8',      'H',      'X',      'h',      'x',      'И',      'Ш',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_9 */ {  c_undef,  c_unmap,      ')',      '9',      'I',      'Y',      'i',      'y',      'Й',      'Щ',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_A */ {  c_undef,      '✓',      '*',      ':',      'J',      'Z',      'j',      'z',      'К',      'Ъ',  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_B */ {  c_undef,  c_undef,      '+',      ';',      'K',      '[',      'k',      '{',      'Л',      'Ы',  c_unmap,  c_undef,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_C */ {  c_undef,  c_undef,      ',',      '<',      'L',     '\\',      'l',      '|',      'М',      'Ь',  c_unmap,  c_undef,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_D */ {  c_undef,  c_undef,      '-',      '=',      'M',      ']',      'm',      '}',      'Н',      'Э',  c_unmap,  c_undef,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_E */ {  c_undef,  c_undef,      '.',      '>',      'N',      '^',      'n',      '~',      'О',      'Ю',  c_unmap,  c_undef,  c_unmap,  c_unmap,  c_unmap,  c_unmap },
        new[] /* 0x_F */ {  c_undef,  c_undef,      '/',      '?',      'O',      '_',      'o',      '¦',      'П',      'Я',  c_unmap,  c_undef,  c_unmap,      'ß',  c_unmap,  c_unmap },
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
      }
    );

    internal static readonly (char, byte)[] CharacterMapRomC = GenerateSortedCharacterMap(
      new char[][] {
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
        new[] /* 0x_0 */ {      '⌠',      '™',      ' ',      '0',      '@',      'P',      '`',      'p',      'Ç',      'É',  c_undef,      'ｰ',      'ﾀ',      'ﾐ',      'á',      '˙' },
        new[] /* 0x_1 */ {      '⌡',      '†',      '!',      '1',      'A',      'Q',      'a',      'q',      'ü',      'æ',      '｡',      'ｱ',      'ﾁ',      'ﾑ',      'í',      '¨' },
        new[] /* 0x_2 */ {      '∞',      '§',      '"',      '2',      'B',      'R',      'b',      'r',      'é',      'Æ',      '｢',      'ｲ',      'ﾂ',      'ﾒ',      'ó',      '˚' },
        new[] /* 0x_3 */ {      '∇',      '¶',      '#',      '3',      'C',      'S',      'c',      's',      'ȧ',      'ô',      '｣',      'ｳ',      'ﾃ',      'ﾓ',      'ú',      'ˋ' },
        new[] /* 0x_4 */ {      '↵',      'Γ',      '$',      '4',      'D',      'T',      'd',      't',      'ä',      'ö',      '､',      'ｴ',      'ﾄ',      'ﾔ',      '¢',      '´' },
        new[] /* 0x_5 */ {      '￪',      'Δ',      '%',      '5',      'E',      'U',      'e',      'u',      'à',      'ò',      '･',      'ｵ',      'ﾅ',      'ﾕ',      '£',      '½' },
        new[] /* 0x_6 */ {      '￬',      'Θ',      '&',      '6',      'F',      'V',      'f',      'v',      'ȧ',      'û',      'ｦ',      'ｶ',      'ﾆ',      'ﾖ',      '¥',      '¼' },
        new[] /* 0x_7 */ {      '￫',      'Λ',     '\'',      '7',      'G',      'W',      'g',      'w',      'ç',      'ù',      'ｧ',      'ｷ',      'ﾇ',      'ﾗ', /*!*/'㌮',      '×' },
        new[] /* 0x_8 */ {      '￩',      'Ξ',      '(',      '8',      'H',      'X',      'h',      'x',      'ê',      'ÿ',      'ｨ',      'ｸ',      'ﾈ',      'ﾘ',      'ƒ',      '÷' },
        new[] /* 0x_9 */ {      '┌',      'Π',      ')',      '9',      'I',      'Y',      'i',      'y',      'ë',      'Ö',      'ｩ',      'ｹ',      'ﾉ',      'ﾙ',  c_unmap,      '≥' },
        new[] /* 0x_A */ {      '┐',      'Σ',      '*',      ':',      'J',      'Z',      'j',      'z',      'è',      'Ü',      'ｪ',      'ｺ',      'ﾊ',      'ﾚ',      'Ã',      '≤' },
        new[] /* 0x_B */ {      '└',      'Υ',      '+',      ';',      'K',      '[',      'k',      '{',      'ï',      'ñ',      'ｫ',      'ｻ',      'ﾋ',      'ﾛ',      'ã',      '≪' },
        new[] /* 0x_C */ {      '┘',      'Φ',      ',',      '<',      'L',     '\\',      'l',      '|',      'î',      'Ñ',      'ｬ',      'ｼ',      'ﾌ',      'ﾜ',      'Õ',      '≫' },
        new[] /* 0x_D */ {      '･',      'Ψ',      '-',      '=',      'M',      ']',      'm',      '}',      'ì',  c_unmap,      'ｭ',      'ｽ',      'ﾍ',      'ﾝ',      'õ',      '≠' },
        new[] /* 0x_E */ {      '®',      'Ω',      '.',      '>',      'N',      '^',      'n',      '￫',      'Ä',  c_unmap,      'ｮ',      'ｾ',      'ﾎ',       'ﾞ',      'Ø',      '√' },
        new[] /* 0x_F */ {      '©',      'α',      '/',      '?',      'O',      '_',      'o',      '￩',      'Â',      '¿',      'ｯ',      'ｿ',      'ﾏ',       'ﾟ',      'ø',      '‾' },
                          /*    0x0_      0x1_      0x2_      0x3_      0x4_      0x5_      0x6_      0x7_      0x8_      0x9_      0xA_      0xB_      0xC_      0xD_      0xE_      0xF_ */
      }
    );

    internal static readonly IComparer<(char, string)> CollationMapEntryComparer =
      Comparer<(char, string)>.Create(static ((char from, string to) x, (char from, string to) y) => Comparer<char>.Default.Compare(x.from, y.from));

    internal static readonly (char from, string to)[] CollationMap = GenerateSortedCollationMap(
      new[] {
        /* to string | from chars */
        /* ---------------------- */
        // Fullwidth ASCII variants
        ("!", new[] {'！'}),
        ("\"", new[] {'＂'}),
        ("#", new[] {'＃'}),
        ("$", new[] {'＄'}),
        ("%", new[] {'％'}),
        ("&", new[] {'＆'}),
        ("'", new[] {'＇'}),
        ("(", new[] {'（'}),
        (")", new[] {'）'}),
        ("*", new[] {'＊'}),
        ("+", new[] {'＋'}),
        (",", new[] {'，'}),
        ("-", new[] {'－'}),
        (".", new[] {'．'}),
        ("/", new[] {'／'}),
        ("0", new[] {'０'}),
        ("1", new[] {'１'}),
        ("2", new[] {'２'}),
        ("3", new[] {'３'}),
        ("4", new[] {'４'}),
        ("5", new[] {'５'}),
        ("6", new[] {'６'}),
        ("7", new[] {'７'}),
        ("8", new[] {'８'}),
        ("9", new[] {'９'}),
        (":", new[] {'：'}),
        (";", new[] {'；'}),
        ("<", new[] {'＜'}),
        ("=", new[] {'＝'}),
        ("<", new[] {'＞'}),
        ("?", new[] {'？'}),
        ("@", new[] {'＠'}),
        ("A", new[] {'Ａ'}),
        ("B", new[] {'Ｂ'}),
        ("C", new[] {'Ｃ'}),
        ("D", new[] {'Ｄ'}),
        ("E", new[] {'Ｅ'}),
        ("F", new[] {'Ｆ'}),
        ("G", new[] {'Ｇ'}),
        ("H", new[] {'Ｈ'}),
        ("I", new[] {'Ｉ'}),
        ("J", new[] {'Ｊ'}),
        ("K", new[] {'Ｋ'}),
        ("L", new[] {'Ｌ'}),
        ("M", new[] {'Ｍ'}),
        ("N", new[] {'Ｎ'}),
        ("O", new[] {'Ｏ'}),
        ("P", new[] {'Ｐ'}),
        ("Q", new[] {'Ｑ'}),
        ("R", new[] {'Ｒ'}),
        ("S", new[] {'Ｓ'}),
        ("T", new[] {'Ｔ'}),
        ("U", new[] {'Ｕ'}),
        ("V", new[] {'Ｖ'}),
        ("W", new[] {'Ｗ'}),
        ("X", new[] {'Ｘ'}),
        ("Y", new[] {'Ｙ'}),
        ("Z", new[] {'Ｚ'}),
        ("[", new[] {'［'}),
        ("\\", new[] {'＼'}),
        ("]", new[] {'］'}),
        ("^", new[] {'＾'}),
        ("_", new[] {'＿'}),
        ("`", new[] {'｀'}),
        ("a", new[] {'ａ'}),
        ("b", new[] {'ｂ'}),
        ("c", new[] {'ｃ'}),
        ("d", new[] {'ｄ'}),
        ("e", new[] {'ｅ'}),
        ("f", new[] {'ｆ'}),
        ("g", new[] {'ｇ'}),
        ("h", new[] {'ｈ'}),
        ("i", new[] {'ｉ'}),
        ("j", new[] {'ｊ'}),
        ("k", new[] {'ｋ'}),
        ("l", new[] {'ｌ'}),
        ("m", new[] {'ｍ'}),
        ("n", new[] {'ｎ'}),
        ("o", new[] {'ｏ'}),
        ("p", new[] {'ｐ'}),
        ("q", new[] {'ｑ'}),
        ("r", new[] {'ｒ'}),
        ("s", new[] {'ｓ'}),
        ("t", new[] {'ｔ'}),
        ("u", new[] {'ｕ'}),
        ("v", new[] {'ｖ'}),
        ("w", new[] {'ｗ'}),
        ("x", new[] {'ｘ'}),
        ("y", new[] {'ｙ'}),
        ("z", new[] {'ｚ'}),
        ("{", new[] {'｛'}),
        ("|", new[] {'｜'}),
        ("}", new[] {'｝'}),
        ("~", new[] {'～'}),

        // Halfwidth CJK punctuation
        ("｡", new[] {'。'}),
        ("｢", new[] {'「'}),
        ("｣", new[] {'」'}),
        ("､", new[] {'、'}),

        // Fullwidth symbol variants
        ("¢", new[] {'￠', '㌣'}),
        ("£", new[] {'￡', '㍀'}),
        ("¥", new[] {'￥'}),
        ("₩", new[] {'￦', '㌆'}),
        ("$", new[] {'＄', '㌦'}),

        // Halfwidth symbol variants
        ("￩", new[] {'←', '⇦'}),
        ("￪", new[] {'↑', '⇧'}),
        ("￫", new[] {'→', '⇨'}),
        ("￬", new[] {'↓', '⇩'}),

        // Geometric shapes
        ("▲", new[] {'△', '▴', '▵', '⏶'}),
        ("▶", new[] {'▷', '▸', '▹', '►', '▻', '⊳', '⏴'}),
        ("▼", new[] {'▽', '▾', '▿', '⏷'}),
        ("◀", new[] {'◁', '◂', '◃', '◄', '◅', '⊲', '⏵'}),

        // User interface symbols
        ("⏩", new[] {'⏭'}),
        ("⏪", new[] {'⏮'}),

        // Box Drawing
        ("┌", new[] {'┏', '╔'}),
        ("┐", new[] {'┓', '╗'}),
        ("└", new[] {'┗', '╚'}),
        ("┘", new[] {'┛', '╝'}),

        // miscellaneous symbols
        (" ", new[] {'　'}),
        ("≤", new[] {'≦', '⩽'}),
        ("≥", new[] {'≧', '⩾'}),
        ("Σ", new[] {'∑'}),
        ("‾", new[] {'￣'}),
        ("･", new[] {'・'}),
        ("´", new[] {'ˊ'}),
        ("¦", new[] {'￤', '︙', '⋮'}),
        ("↵", new[] {'↲', '⏎'}),
        ("✓", new[] {'☑', '✔'}),
        ("♥", new[] {'♡', '❤'}),
      }
    );

    private static (char, string)[] GenerateSortedCollationMap((string to, char[] from)[] table)
    {
      var sortedMap = new List<(char from, string to)>();

      foreach (var entry in table) {
        foreach (var from in entry.from) {
          sortedMap.Add((from, entry.to));
        }
      }

      sortedMap.Sort(CollationMapEntryComparer);

      return sortedMap.ToArray();
    }
  }
}