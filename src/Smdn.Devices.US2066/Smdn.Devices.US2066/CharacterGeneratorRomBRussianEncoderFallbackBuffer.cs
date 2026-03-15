// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Devices.US2066;

public class CharacterGeneratorRomBRussianEncoderFallbackBuffer : CharacterGeneratorEncoderFallbackBuffer {
  public CharacterGeneratorRomBRussianEncoderFallbackBuffer(CharacterGeneratorRomBRussianEncoderFallback fallback)
    : base(fallback)
  {
  }

  protected override string GetReplacement(char charUnknown)
  {
    return charUnknown switch {
      // CYRILLIC SMALL LETTER -> CYRILLIC CAPITAL LETTER
      'а' => "А",
      'б' => "Б",
      'в' => "В",
      'г' => "Г",
      'д' => "Д",
      'е' => "Е",
      'ж' => "Ж",
      'з' => "З",
      'и' => "И",
      'й' => "Й",
      'к' => "К",
      'л' => "Л",
      'м' => "М",
      'н' => "Н",
      'о' => "О",
      'п' => "П",
      'р' => "Р",
      'с' => "С",
      'т' => "Т",
      'у' => "У",
      'ф' => "Ф",
      'х' => "Х",
      'ц' => "Ц",
      'ч' => "Ч",
      'ш' => "Ш",
      'щ' => "Щ",
      'ъ' => "Ъ",
      'ы' => "Ы",
      'ь' => "Ь",
      'э' => "Э",
      'ю' => "Ю",
      'я' => "Я",

      // Cyrillic extensions
      'ѐ' => "è",
      'Ѐ' => "È",
      'ё' => "ë",
      'Ё' => "Ë",
      'ђ' or 'Ђ' => DefaultReplacementString,
      'ѓ' or 'Ѓ' => DefaultReplacementString,
      'є' or 'Є' => DefaultReplacementString,
      'ѕ' => "s",
      'Ѕ' => "S",
      'і' => "i",
      'І' => "I",
      'ї' => "ï",
      'Ї' => "Ï",
      'ј' => "j",
      'Ј' => "J",
      'љ' or 'Љ' => DefaultReplacementString,
      'њ' or 'Њ' => DefaultReplacementString,
      'ћ' or 'Ћ' => DefaultReplacementString,
      'ќ' or 'Ќ' => DefaultReplacementString,
      'ѝ' or 'Ѝ' => DefaultReplacementString,
      'ў' or 'Ў' => DefaultReplacementString,
      'џ' or 'Џ' => DefaultReplacementString,

      _ => base.GetReplacement(charUnknown),
    };
  }
}
