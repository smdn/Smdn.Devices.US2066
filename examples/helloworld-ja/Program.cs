// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

// CGRomCJapanese supports Japanese full-width katakana and hiragana letters.
// The full-width katakana and hiragana letters will be displayed as half-width katakana characters.
// CGRomCJapaneseを使用すると、全角かなと全角カナは自動的に半角カナに変換された上で表示されます。
display.CharacterGenerator = CharacterGeneratorEncoding.CGRomCJapanese;

display.WriteLine("こんにちは、せかい！");
display.WriteLine("「おハヨウ　ｺﾞざいます。」");

