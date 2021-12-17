// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Devices.US2066;

internal class CharacterGeneratorRomCJapaneseEncoderFallbackBuffer : CharacterGeneratorEncoderFallbackBuffer {
  internal CharacterGeneratorRomCJapaneseEncoderFallbackBuffer(CharacterGeneratorRomCJapaneseEncoderFallback fallback)
    : base(fallback)
  {
  }

  protected override string GetReplacement(char charUnknown)
  {
    return charUnknown switch {
      '　' => "\uFF60",
      '・' => "\uFF65",

      'を' or 'ヲ' => "\uFF66",
      'ヺ' => "\uFF66\uFF9E",

      'ぁ' or 'ァ' => "\uFF67",
      'ぃ' or 'ィ' => "\uFF68",
      'ぅ' or 'ゥ' => "\uFF69",
      'ぇ' or 'ェ' => "\uFF6A",
      'ぉ' or 'ォ' => "\uFF6B",

      'ゃ' or 'ャ' => "\uFF6C",
      'ゅ' or 'ュ' => "\uFF6D",
      'ょ' or 'ョ' => "\uFF6E",
      'っ' or 'ッ' => "\uFF6F",

      'ー' => "\uFFF0",

      'あ' or 'ア' => "\uFF71",
      'い' or 'イ' => "\uFF72",
      'う' or 'ウ' => "\uFF73",
      'え' or 'エ' => "\uFF74",
      'お' or 'オ' => "\uFF75",

      'ゔ' or 'ヴ' => "\uFF73\uFF9E",

      'か' or 'カ' => "\uFF76",
      'き' or 'キ' => "\uFF77",
      'く' or 'ク' => "\uFF78",
      'け' or 'ケ' => "\uFF79",
      'こ' or 'コ' => "\uFF7A",

      'ゕ' or 'ヵ' => DefaultReplacementString,
      'ゖ' or 'ヶ' => DefaultReplacementString,

      'が' or 'ガ' => "\uFF76\uFF9E",
      'ぎ' or 'ギ' => "\uFF77\uFF9E",
      'ぐ' or 'グ' => "\uFF78\uFF9E",
      'げ' or 'ゲ' => "\uFF79\uFF9E",
      'ご' or 'ゴ' => "\uFF7A\uFF9E",

      'さ' or 'サ' => "\uFF7B",
      'し' or 'シ' => "\uFF7C",
      'す' or 'ス' => "\uFF7D",
      'せ' or 'セ' => "\uFF7E",
      'そ' or 'ソ' => "\uFF7F",

      'ざ' or 'ザ' => "\uFF7B\uFF9E",
      'じ' or 'ジ' => "\uFF7C\uFF9E",
      'ず' or 'ズ' => "\uFF7D\uFF9E",
      'ぜ' or 'ゼ' => "\uFF7E\uFF9E",
      'ぞ' or 'ゾ' => "\uFF7F\uFF9E",

      'た' or 'タ' => "\uFF80",
      'ち' or 'チ' => "\uFF81",
      'つ' or 'ツ' => "\uFF82",
      'て' or 'テ' => "\uFF83",
      'と' or 'ト' => "\uFF84",

      'だ' or 'ダ' => "\uFF80\uFF9E",
      'ぢ' or 'ヂ' => "\uFF81\uFF9E",
      'づ' or 'ヅ' => "\uFF82\uFF9E",
      'で' or 'デ' => "\uFF83\uFF9E",
      'ど' or 'ド' => "\uFF84\uFF9E",

      'な' or 'ナ' => "\uFF85",
      'に' or 'ニ' => "\uFF86",
      'ぬ' or 'ヌ' => "\uFF87",
      'ね' or 'ネ' => "\uFF88",
      'の' or 'ノ' => "\uFF89",

      'は' or 'ハ' => "\uFF8A",
      'ひ' or 'ヒ' => "\uFF8B",
      'ふ' or 'フ' => "\uFF8C",
      'へ' or 'ヘ' => "\uFF8D",
      'ほ' or 'ホ' => "\uFF8E",

      'ば' or 'バ' => "\uFF8A\uFF9E",
      'び' or 'ビ' => "\uFF8B\uFF9E",
      'ぶ' or 'ブ' => "\uFF8C\uFF9E",
      'べ' or 'ベ' => "\uFF8D\uFF9E",
      'ぼ' or 'ボ' => "\uFF8E\uFF9E",

      'ぱ' or 'パ' => "\uFF8A\uFF9F",
      'ぴ' or 'ピ' => "\uFF8B\uFF9F",
      'ぷ' or 'プ' => "\uFF8C\uFF9F",
      'ぺ' or 'ペ' => "\uFF8D\uFF9F",
      'ぽ' or 'ポ' => "\uFF8E\uFF9F",

      'ま' or 'マ' => "\uFF8F",
      'み' or 'ミ' => "\uFF90",
      'む' or 'ム' => "\uFF91",
      'め' or 'メ' => "\uFF92",
      'も' or 'モ' => "\uFF93",

      'や' or 'ヤ' => "\uFF94",
      'ゆ' or 'ユ' => "\uFF95",
      'よ' or 'ヨ' => "\uFF96",

      'ら' or 'ラ' => "\uFF97",
      'り' or 'リ' => "\uFF98",
      'る' or 'ル' => "\uFF99",
      'れ' or 'レ' => "\uFF9A",
      'ろ' or 'ロ' => "\uFF9B",

      'わ' or 'ワ' => "\uFF9C",
      'ん' or 'ン' => "\uFF9D",

      'ゎ' or 'ヮ' => DefaultReplacementString,

      'ヷ' => "\uFF9C\uFF9E",

      '゛' => "\uFF9E",
      '゜' => "\uFF9F",

      // Katakana Phonetic Extensions
      'ㇰ' => DefaultReplacementString,
      'ㇱ' => DefaultReplacementString,
      'ㇲ' => DefaultReplacementString,
      'ㇳ' => DefaultReplacementString,
      'ㇴ' => DefaultReplacementString,
      'ㇵ' => DefaultReplacementString,
      'ㇶ' => DefaultReplacementString,
      'ㇷ' => DefaultReplacementString,
      'ㇸ' => DefaultReplacementString,
      'ㇹ' => DefaultReplacementString,
      'ㇺ' => DefaultReplacementString,
      'ㇻ' => DefaultReplacementString,
      'ㇼ' => DefaultReplacementString,
      'ㇽ' => DefaultReplacementString,
      'ㇾ' => DefaultReplacementString,
      'ㇿ' => DefaultReplacementString,

      _ => base.GetReplacement(charUnknown),
    };
  }
}
