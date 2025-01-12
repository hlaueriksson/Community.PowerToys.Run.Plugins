namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    /// <summary>
    /// Contains auto-generated JSON of all methods.
    /// </summary>
    public static partial class Constants
    {
        public const string Methods =
"""
{
  "all.all": {
    "Key": "all.all",
    "Method": "All",
    "Title": "Encoding \u0026 Decoding Online Tools",
    "Description": "Encoding and Decoding site. e.g. HTML Escape / URL Encoding / Base64 / MD5 / SHA-1 / CRC32 / and many other String, Number, DateTime, Color, Hash formats!",
    "Tooltip": "Enter the value to be converted. (e.g. String: \u0022Hello!\u0022 / Number: \u00221234.5\u0022 / Date: \u00221984-02-07T12:34:56\u0022 / Color: \u0022rgb(255 0 0)\u0022)",
    "Label": {}
  },
  "string.all": {
    "Key": "string.all",
    "Method": "String - All",
    "Title": "String Encoder / Decoder, Converter Online",
    "Description": "String encoding and decoding converter. e.g. HTML Escape / URL Encoding / Quoted-printable / and many other formats!",
    "Tooltip": "Enter the value to be converted. (e.g. \u0022Hello, world!\u0022)",
    "Label": {}
  },
  "string.bin": {
    "Key": "string.bin",
    "Method": "Bin String",
    "Title": "Bin to String Converter Online",
    "Description": "Bin to String converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002201001000 01100101 01101100 01101100 01101111 00101100 00100000 01110111 01101111 01110010 01101100 01100100 00100001\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002201001000 01100101 01101100 01101100 01101111 00101100 00100000 01110111 01101111 01110010 01101100 01100100 00100001\u0022)",
    "Label": {
      "encStrBin": "Bin String",
      "decStrBin": "Bin String"
    }
  },
  "string.hex": {
    "Key": "string.hex",
    "Method": "Hex String",
    "Title": "Hex to String Converter Online",
    "Description": "Hex to String converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002248 65 6C 6C 6F 2C 20 77 6F 72 6C 64 21\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002248 65 6C 6C 6F 2C 20 77 6F 72 6C 64 21\u0022)",
    "Label": {
      "encStrHex": "Hex String",
      "decStrHex": "Hex String"
    }
  },
  "string.html-escape": {
    "Key": "string.html-escape",
    "Method": "HTML Escape",
    "Title": "HTML Escape / Unescape (Encoder / Decoder) Online",
    "Description": "HTML Escape / Unescape (encoder / decoder). (e.g. \u0022\u003Cp\u003EHello, world!\u003C/p\u003E\u0022 \u003C=\u003E \u0022\u0026lt;p\u0026gt;Hello, world!\u0026lt;/p\u0026gt;\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022\u003Cp\u003EHello, world!\u003C/p\u003E\u0022 \u003C=\u003E \u0022\u0026lt;p\u0026gt;Hello, world!\u0026lt;/p\u0026gt;\u0022)",
    "Label": {
      "encStrHTMLEscape": "HTML Escape (Basic)",
      "encStrHTMLEscapeFully": "HTML Escape (Fully)",
      "decStrHTMLEscape": "HTML Escape"
    }
  },
  "string.url-encoding": {
    "Key": "string.url-encoding",
    "Method": "URL Encoding",
    "Title": "URL Encoding Encoder / Decoder Online",
    "Description": "URL Encoding encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022Hello%2C%20world%21\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022Hello%2C%20world%21\u0022)",
    "Label": {
      "encStrURLEncoding": "URL Encoding",
      "decStrURLEncoding": "URL Encoding"
    }
  },
  "string.punycode": {
    "Key": "string.punycode",
    "Method": "Punycode IDN",
    "Title": "Punycode IDN domain name Converter Online",
    "Description": "Punycode IDN domain name converter. (e.g. \u0022\u30C9\u30E1\u30A4\u30F3.com\u0022 \u003C=\u003E \u0022xn--eckwd4c7c.com\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022\u30C9\u30E1\u30A4\u30F3.com\u0022 \u003C=\u003E \u0022xn--eckwd4c7c.com\u0022)",
    "Label": {
      "encStrPunycode": "Punycode IDN",
      "decStrPunycode": "Punycode IDN"
    }
  },
  "string.base32": {
    "Key": "string.base32",
    "Method": "Base32",
    "Title": "Base32 Encoder / Decoder Online",
    "Description": "Base32 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022JBSWY3DPFQQHO33SNRSCC===\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022JBSWY3DPFQQHO33SNRSCC===\u0022)",
    "Label": {
      "encStrBase32": "Base32",
      "decStrBase32": "Base32"
    }
  },
  "string.base45": {
    "Key": "string.base45",
    "Method": "Base45",
    "Title": "Base45 Encoder / Decoder Online",
    "Description": "Base45 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022%69 VDK2EV4404ESVDX0\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022%69 VDK2EV4404ESVDX0\u0022)",
    "Label": {
      "encStrBase45": "Base45",
      "decStrBase45": "Base45",
      "decStrBase45ZlibCoseCbor": "Base45/Zlib/COSE/CBOR"
    }
  },
  "string.base64": {
    "Key": "string.base64",
    "Method": "Base64",
    "Title": "Base64 Encoder / Decoder Online",
    "Description": "Base64 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022SGVsbG8sIHdvcmxkIQ==\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022SGVsbG8sIHdvcmxkIQ==\u0022)",
    "Label": {
      "encStrBase64": "Base64",
      "decStrBase64": "Base64"
    }
  },
  "string.ascii85": {
    "Key": "string.ascii85",
    "Method": "Ascii85",
    "Title": "Ascii85 Z85/Adobe/btoa Encoder / Decoder Online",
    "Description": "Ascii85 Z85/Adobe/btoa encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022nm=QNz.92Pz/PV8aP\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022nm=QNz.92Pz/PV8aP\u0022)",
    "Label": {
      "encStrAscii85": "Ascii85",
      "decStrAscii85": "Ascii85"
    }
  },
  "string.quoted-printable": {
    "Key": "string.quoted-printable",
    "Method": "Quoted-printable",
    "Title": "Quoted-printable Encoder / Decoder Online",
    "Description": "Quoted-printable encoder / decoder. (e.g. \u0022Hello, != world!\u0022 \u003C=\u003E \u0022Hello, !=3D world!\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, != world!\u0022 \u003C=\u003E \u0022Hello, !=3D world!\u0022)",
    "Label": {
      "encStrQuotedPrintable": "Quoted-printable",
      "decStrQuotedPrintable": "Quoted-printable"
    }
  },
  "string.unicode-escape": {
    "Key": "string.unicode-escape",
    "Method": "Unicode Escape",
    "Title": "Unicode Escape (\\\\u %u \\\\x \u0026#x U\u002B 0x \\\\N) Encoder / Decoder Online",
    "Description": "Unicode Escape encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\\\\u0048\\\\u0065\\\\u006c\\\\u006c\\\\u006f\\\\u002c\\\\u0020\\\\u0077\\\\u006f\\\\u0072\\\\u006c\\\\u0064\\\\u0021\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\\\\u0048\\\\u0065\\\\u006c\\\\u006c\\\\u006f\\\\u002c\\\\u0020\\\\u0077\\\\u006f\\\\u0072\\\\u006c\\\\u0064\\\\u0021\u0022)",
    "Label": {
      "encStrUnicodeEscape": "Unicode Escape",
      "decStrUnicodeEscape": "Unicode Escape"
    }
  },
  "string.program-string": {
    "Key": "string.program-string",
    "Method": "Program String",
    "Title": "Program String Escape / Unescape Online",
    "Description": "Program String escape / unescape. (e.g. \u0022Hello,\tworld!\u0022 \u003C=\u003E \u0022\u0022Hello,\\\\tworld!\u0022\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello,\tworld!\u0022 \u003C=\u003E \u0022\u0022Hello,\\\\tworld!\u0022\u0022)",
    "Label": {
      "encStrProgramString": "Program String",
      "decStrProgramString": "Program String"
    }
  },
  "string.morse-code": {
    "Key": "string.morse-code",
    "Method": "Morse Code",
    "Title": "Morse Code Translator Online",
    "Description": "Morse Code translator. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022.... . .-.. .-.. --- --..-- / .-- --- .-. .-.. -.. -.-.--\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022.... . .-.. .-.. --- --..-- / .-- --- .-. .-.. -.. -.-.--\u0022)",
    "Label": {
      "encStrMorseCode": "Morse Code",
      "decStrMorseCode": "Morse Code"
    }
  },
  "string.character-width": {
    "Key": "string.character-width",
    "Method": "Character Width (Half, Full)",
    "Title": "Character Width (Half-width, Full-width) Converter Online",
    "Description": "Character Width (Half-width, Full-width) converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\uFF28\uFF45\uFF4C\uFF4C\uFF4F\uFF0C\u3000\uFF57\uFF4F\uFF52\uFF4C\uFF44\uFF01\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\uFF28\uFF45\uFF4C\uFF4C\uFF4F\uFF0C\u3000\uFF57\uFF4F\uFF52\uFF4C\uFF44\uFF01\u0022)",
    "Label": {
      "encStrHalfWidth": "Half Width",
      "encStrFullWidth": "Full Width"
    }
  },
  "string.letter-case": {
    "Key": "string.letter-case",
    "Method": "Letter Case (Upper, Lower, Swap, Capital)",
    "Title": "Letter Case (Upper-case, Lower-case) Converter Online",
    "Description": "Letter Case (Upper-case, Lower-case) converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO, WORLD!\u0022, \u0022hello, world!\u0022, \u0022hELLO, WORLD!\u0022, \u0022Hello, World!\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO, WORLD!\u0022, \u0022hello, world!\u0022, \u0022hELLO, WORLD!\u0022, \u0022Hello, World!\u0022)",
    "Label": {
      "encStrUpperCase": "Upper Case",
      "encStrLowerCase": "Lower Case",
      "encStrSwapCase": "Swap Case",
      "encStrCapitalize": "Capitalize"
    }
  },
  "string.text-initials": {
    "Key": "string.text-initials",
    "Method": "Text Initials",
    "Title": "Initials Text Converter Online",
    "Description": "Initials Text converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hw\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hw\u0022)",
    "Label": {
      "encStrInitials": "Initials"
    }
  },
  "string.text-reverse": {
    "Key": "string.text-reverse",
    "Method": "Text Reverse",
    "Title": "Reverse Text Converter Online",
    "Description": "Reverse Text converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022!dlrow ,olleH\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022!dlrow ,olleH\u0022)",
    "Label": {
      "encStrReverse": "Reverse"
    }
  },
  "string.naming-convention": {
    "Key": "string.naming-convention",
    "Method": "Naming Convention",
    "Title": "Camel Case / Snake Case / Kebab Case / Naming Convention Converter Online",
    "Description": "Camel Case / Snake Case / Kebab Case / Naming Convention converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022, \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022, \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022, \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022, \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
    "Label": {}
  },
  "string.camel-case": {
    "Key": "string.camel-case",
    "Method": "Camel Case",
    "Title": "Camel Case (Pascal Case) Converter Online",
    "Description": "Camel Case (Pascal Case) converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022)",
    "Label": {
      "encStrUpperCamelCase": "UpperCamelCase",
      "encStrLowerCamelCase": "lowerCamelCase"
    }
  },
  "string.snake-case": {
    "Key": "string.snake-case",
    "Method": "Snake Case",
    "Title": "Snake Case Converter Online",
    "Description": "Snake Case converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022)",
    "Label": {
      "encStrUpperSnakeCase": "UPPER_SNAKE_CASE",
      "encStrLowerSnakeCase": "lower_snake_case"
    }
  },
  "string.kebab-case": {
    "Key": "string.kebab-case",
    "Method": "Kebab Case",
    "Title": "Kebab Case Converter Online",
    "Description": "Kebab Case converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
    "Label": {
      "encStrUpperKebabCase": "UPPER-KEBAB-CASE",
      "encStrLowerKebabCase": "lower-kebab-case"
    }
  },
  "string.font-style": {
    "Key": "string.font-style",
    "Method": "Font Style",
    "Title": "Font Style Converter Online",
    "Description": "Font style converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022\uD835\uDC3B\uD835\uDC52\uD835\uDCC1\uD835\uDCC1\uD835\uDC5C, \uD835\uDCCC\uD835\uDC5C\uD835\uDCC7\uD835\uDCC1\uD835\uDCB9!\u0022)",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022\uD835\uDC3B\uD835\uDC52\uD835\uDCC1\uD835\uDCC1\uD835\uDC5C, \uD835\uDCCC\uD835\uDC5C\uD835\uDCC7\uD835\uDCC1\uD835\uDCB9!\u0022)",
    "Label": {
      "encStrFontStyle": "Font Style"
    }
  },
  "string.unicode-normalization": {
    "Key": "string.unicode-normalization",
    "Method": "Unicode Normalization",
    "Title": "Unicode Normalization (NFC, NFKC, NFD, NFKD) Converter Online",
    "Description": "Unicode Normalization (NFC, NFKC, NFD, NFKD) converter.",
    "Tooltip": "Enter the text to be converted.",
    "Label": {
      "encStrUnicodeNFC": "Unicode NFC",
      "encStrUnicodeNFKC": "Unicode NFKC",
      "decStrUnicodeNFC": "Unicode NFD",
      "decStrUnicodeNFKC": "Unicode NFKD"
    }
  },
  "string.line-sort": {
    "Key": "string.line-sort",
    "Method": "Line Sort",
    "Title": "Text Line Sorter (Asc, Desc, Reverse) Online",
    "Description": "Text Line Sorter (Asc, Desc, Reverse).",
    "Tooltip": "Enter the multi-line text to be converted.",
    "Label": {
      "encStrLineSort": "Line Sort"
    }
  },
  "string.line-unique": {
    "Key": "string.line-unique",
    "Method": "Line Unique",
    "Title": "Text Line Duplicates Remover Online",
    "Description": "Text Line Duplicates Remover.",
    "Tooltip": "Enter the multi-line text to be converted.",
    "Label": {
      "encStrLineUnique": "Line Unique"
    }
  },
  "number.all": {
    "Key": "number.all",
    "Method": "Number - All",
    "Title": "Number Converter Online",
    "Description": "Number converter. e.g. Binary numbers / Octal numbers / Decimal numbers / Hexadecimal numbers / English words numerals / Japanese numerals / and many other formats!",
    "Tooltip": "Enter the number to be converted. (e.g. Dec: \u00221234.5\u0022 / Bin: \u002210011010010.1\u0022 / Oct: \u00222322.4\u0022 / Hex: \u00224d2.8\u0022 / EN: \u0022One Thousand Two Hundred Thirty-Four point Five\u0022 / JP: \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022)",
    "Label": {}
  },
  "number.dec": {
    "Key": "number.dec",
    "Method": "Decimal Numbers",
    "Title": "Decimal Numbers Converter Online",
    "Description": "Decimal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00221234.5\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00221234.5\u0022)",
    "Label": {
      "encNumDec": "Num to Dec",
      "decNumDec": "Num from Dec"
    }
  },
  "number.bin": {
    "Key": "number.bin",
    "Method": "Binary Numbers",
    "Title": "Binary Numbers Converter Online",
    "Description": "Binary numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u002210011010010.1\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u002210011010010.1\u0022)",
    "Label": {
      "encNumBin": "Num to Bin",
      "decNumBin": "Num from Bin"
    }
  },
  "number.oct": {
    "Key": "number.oct",
    "Method": "Octal Numbers",
    "Title": "Octal Numbers Converter Online",
    "Description": "Octal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00222322.4\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00222322.4\u0022)",
    "Label": {
      "encNumOct": "Num to Oct",
      "decNumOct": "Num from Oct"
    }
  },
  "number.hex": {
    "Key": "number.hex",
    "Method": "Hexadecimal Numbers",
    "Title": "Hexadecimal Numbers Converter Online",
    "Description": "Hexadecimal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00224d2.8\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00224d2.8\u0022)",
    "Label": {
      "encNumHex": "Num to Hex",
      "decNumHex": "Num from Hex"
    }
  },
  "number.n-ary": {
    "Key": "number.n-ary",
    "Method": "N-ary Numbers",
    "Title": "N-ary Numbers Converter Online",
    "Description": "N-ary numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u002216i.g\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u002216i.g\u0022)",
    "Label": {
      "encNumNAry": "Num to N-ary",
      "decNumNAry": "Num from N-ary"
    }
  },
  "number.english": {
    "Key": "number.english",
    "Method": "English Numerals",
    "Title": "English Words Numerals Converter Online",
    "Description": "English words numerals converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u0022One Thousand Two Hundred Thirty-Four point Five\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u0022One Thousand Two Hundred Thirty-Four point Five\u0022)",
    "Label": {
      "encNumEnShortScale": "Num to English",
      "decNumEnShortScale": "Num from English"
    }
  },
  "number.japanese": {
    "Key": "number.japanese",
    "Method": "Kanji Numerals",
    "Title": "Japanese Kanji Numerals Converter Online",
    "Description": "Japanese Kanji numerals converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022",
    "Tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022)",
    "Label": {
      "encNumJP": "Num to Kanji",
      "encNumJPDaiji": "Num to Kanji Daiji",
      "decNumJP": "Num from Kanji"
    }
  },
  "date.all": {
    "Key": "date.all",
    "Method": "Date - All",
    "Title": "Date Time Converter Online",
    "Description": "Date Time converter. e.g. UNIX Time / ISO8601 Date / RFC2822 Date / and many other formats!",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022 / ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 / RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022)",
    "Label": {}
  },
  "date.unix-time": {
    "Key": "date.unix-time",
    "Method": "UNIX Time",
    "Title": "UNIX Time (POSIX Time) Converter Online",
    "Description": "UNIX time (POSIX time) converter. e.g. ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E UNIX Time: \u0022444972896.789\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E UNIX Time: \u0022444972896.789\u0022)",
    "Label": {
      "encDateUnixTime": "UNIX Time [sec]"
    }
  },
  "date.w3cdtf": {
    "Key": "date.w3cdtf",
    "Method": "W3C-DTF Date",
    "Title": "W3C-DTF Date Converter Online",
    "Description": "W3C-DTF date converter. e.g. UNIX Time: \u0022444972896.789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E W3C-DTF: \u00221984-02-07T12:34:56.789\u002B09:00\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E W3C-DTF: \u00221984-02-07T12:34:56.789\u002B09:00\u0022)",
    "Label": {
      "encDateW3CDTF": "W3C-DTF Date"
    }
  },
  "date.iso8601": {
    "Key": "date.iso8601",
    "Method": "ISO8601 Date",
    "Title": "ISO8601 Date Converter Online",
    "Description": "ISO8601 date converter. e.g. UNIX Time: \u0022444972896.789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022)",
    "Label": {
      "encDateISO8601": "ISO8601 Date",
      "encDateISO8601Ext": "ISO8601 Date (Extend)",
      "encDateISO8601Week": "ISO8601 Date (Week)",
      "encDateISO8601Ordinal": "ISO8601 Date (Ordinal)"
    }
  },
  "date.rfc2822": {
    "Key": "date.rfc2822",
    "Method": "RFC2822 Date",
    "Title": "RFC2822 Date Converter Online",
    "Description": "RFC2822 date converter. e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022)",
    "Label": {
      "encDateRFC2822": "RFC2822 Date"
    }
  },
  "date.ctime": {
    "Key": "date.ctime",
    "Method": "ctime Date",
    "Title": "ctime Date Converter Online",
    "Description": "ctime date converter. e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E ctime: \u0022Tue Feb 07 12:34:56 1984\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E ctime: \u0022Tue Feb 07 12:34:56 1984\u0022)",
    "Label": {
      "encDateCTime": "ctime Date"
    }
  },
  "date.japanese-era": {
    "Key": "date.japanese-era",
    "Method": "Japanese Era",
    "Title": "Japanese Era Date Converter Online",
    "Description": "Japanese Era date converter. e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E Japanese Era: \u0022\u662D\u548C59\u5E7402\u670807\u65E503\u664234\u520656.789\u79D2\u0022",
    "Tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896.789\u0022, ISO8601: \u00221984-02-07T12:34:56.789\u002B09:00\u0022 =\u003E Japanese Era: \u0022\u662D\u548C59\u5E7402\u670807\u65E503\u664234\u520656.789\u79D2\u0022)",
    "Label": {
      "encDateJapaneseEra": "Japanese Era"
    }
  },
  "color.all": {
    "Key": "color.all",
    "Method": "Color - All",
    "Title": "Color Converter Online",
    "Description": "Color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022)",
    "Label": {}
  },
  "color.name": {
    "Key": "color.name",
    "Method": "Color Name",
    "Title": "Color Name Converter Online",
    "Description": "Color name converter. e.g. \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022red\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022red\u0022)",
    "Label": {
      "encColorName": "Color Name"
    }
  },
  "color.rgb": {
    "Key": "color.rgb",
    "Method": "RGB Color",
    "Title": "RGB Color Converter Online",
    "Description": "RGB color converter. e.g. \u0022red\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022)",
    "Label": {
      "encColorRGBHex": "RGB Color (Hex)",
      "encColorRGBFn": "RGB Color"
    }
  },
  "color.hsl": {
    "Key": "color.hsl",
    "Method": "HSL Color",
    "Title": "HSL Color Converter Online",
    "Description": "HSL color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsl(0 100% 50%)\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsl(0 100% 50%)\u0022)",
    "Label": {
      "encColorHSLFn": "HSL Color"
    }
  },
  "color.hsv": {
    "Key": "color.hsv",
    "Method": "HSV Color",
    "Title": "HSV Color Converter Online",
    "Description": "HSV color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsv(0 100% 100%)\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsv(0 100% 100%)\u0022)",
    "Label": {
      "encColorHSVFn": "HSV Color"
    }
  },
  "color.cmyk": {
    "Key": "color.cmyk",
    "Method": "CMYK Color",
    "Title": "CMYK Color Converter Online",
    "Description": "CMYK color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022 =\u003E \u0022device-cmyk(0% 100% 100% 0%)\u0022",
    "Tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022 =\u003E \u0022device-cmyk(0% 100% 100% 0%)\u0022)",
    "Label": {
      "encColorCMYKFn": "CMYK Color"
    }
  },
  "cipher.all": {
    "Key": "cipher.all",
    "Method": "Cipher - All",
    "Title": "Cipher Encrypter / Decrypter Online",
    "Description": "Cipher encrypter / decrypter. e.g. Caesar / ROT / and many other formats!",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022)",
    "Label": {}
  },
  "cipher.caesar": {
    "Key": "cipher.caesar",
    "Method": "Caesar Cipher",
    "Title": "Caesar Cipher Encrypter / Decrypter Online",
    "Description": "Caesar Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Ebiil, tloia!\u0022)",
    "Label": {
      "encCipherCaesar": "Caesar",
      "decCipherCaesar": "Caesar"
    }
  },
  "cipher.rot13": {
    "Key": "cipher.rot13",
    "Method": "ROT13",
    "Title": "ROT13 Encrypter / Decrypter Online",
    "Description": "ROT13 encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022Uryyb, jbeyq 123!\u0022)",
    "Label": {
      "encCipherROT13": "ROT13 (A-Z)",
      "decCipherROT13": "ROT13 (A-Z)"
    }
  },
  "cipher.rot18": {
    "Key": "cipher.rot18",
    "Method": "ROT18",
    "Title": "ROT18 Encrypter / Decrypter Online",
    "Description": "ROT18 encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022Uryyb, jbeyq 678!\u0022)",
    "Label": {
      "encCipherROT18": "ROT18 (A-Z, 0-9)",
      "decCipherROT18": "ROT18 (A-Z, 0-9)"
    }
  },
  "cipher.rot47": {
    "Key": "cipher.rot47",
    "Method": "ROT47",
    "Title": "ROT47 Encrypter / Decrypter Online",
    "Description": "ROT47 encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022w6==@[ H@C=5 \u0060abP\u0022)",
    "Label": {
      "encCipherROT47": "ROT47 (!-~)",
      "decCipherROT47": "ROT47 (!-~)"
    }
  },
  "cipher.atbash": {
    "Key": "cipher.atbash",
    "Method": "Atbash",
    "Title": "Atbash Cipher Encrypter / Decrypter Online",
    "Description": "Atbash Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Svool, dliow!\u0022)",
    "Label": {
      "encCipherAtbash": "Atbash"
    }
  },
  "cipher.affine": {
    "Key": "cipher.affine",
    "Method": "Affine",
    "Title": "Affine Cipher Encrypter / Decrypter Online",
    "Description": "Affine Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Ebiil, tloia!\u0022)",
    "Label": {
      "encCipherAffine": "Affine",
      "decCipherAffine": "Affine"
    }
  },
  "cipher.vigenere": {
    "Key": "cipher.vigenere",
    "Method": "Vigen\u00E8re",
    "Title": "Vigen\u00E8re Cipher Encrypter / Decrypter Online",
    "Description": "Vigen\u00E8re Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Zincs, aqipw!\u0022)",
    "Label": {
      "encCipherVigenere": "Vigen\u00E8re",
      "decCipherVigenere": "Vigen\u00E8re"
    }
  },
  "cipher.enigma": {
    "Key": "cipher.enigma",
    "Method": "Enigma",
    "Title": "Enigma Machine Simulator Online",
    "Description": "Enigma Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Kcubr, kidkn!\u0022)",
    "Label": {
      "encCipherEnigma": "Enigma",
      "decCipherEnigma": "Enigma"
    }
  },
  "cipher.jis-keyboard": {
    "Key": "cipher.jis-keyboard",
    "Method": "JIS Keyboard",
    "Title": "JIS Keyboard Encrypter / Decrypter Online",
    "Description": "JIS Keyboard encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022ntt\u0022 =\u003E \u0022\u307F\u304B\u304B\u0022)",
    "Label": {
      "encCipherJisKeyboard": "JIS Keyboard"
    }
  },
  "cipher.scytale": {
    "Key": "cipher.scytale",
    "Method": "Scytale Cipher",
    "Title": "Scytale Cipher Encrypter / Decrypter Online",
    "Description": "Scytale Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hweolrllod,! \u0022)",
    "Label": {
      "encCipherScytale": "Scytale",
      "decCipherScytale": "Scytale"
    }
  },
  "cipher.rail-fence": {
    "Key": "cipher.rail-fence",
    "Method": "Rail Fence Cipher",
    "Title": "Rail Fence Cipher Encrypter / Decrypter Online",
    "Description": "Rail Fence Cipher encrypter / decrypter.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hlo ol!el,wrd\u0022)",
    "Label": {
      "encCipherRailFence": "Rail Fence",
      "decCipherRailFence": "Rail Fence"
    }
  },
  "hash.all": {
    "Key": "hash.all",
    "Method": "Hash - All",
    "Title": "Hash Value Calculator Online",
    "Description": "Hash value calculator. e.g. MD2 / MD5 / SHA-1 / SHA-256 / SHA-384 / SHA-512 / CRC32 / and many other formats!",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022)",
    "Label": {}
  },
  "hash.md2": {
    "Key": "hash.md2",
    "Method": "MD2",
    "Title": "MD2 Hash Value Calculator Online",
    "Description": "MD2 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u00228cca0e965edd0e223b744f9cedf8e141\u0022)",
    "Label": {
      "encHashMD2": "MD2"
    }
  },
  "hash.md5": {
    "Key": "hash.md5",
    "Method": "MD5",
    "Title": "MD5 Hash Value Calculator Online",
    "Description": "MD5 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u00226cd3556deb0da54bca060b4c39479839\u0022)",
    "Label": {
      "encHashMD5": "MD5"
    }
  },
  "hash.sha1": {
    "Key": "hash.sha1",
    "Method": "SHA-1",
    "Title": "SHA1 Hash Value Calculator Online",
    "Description": "SHA1 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022943a702d06f34599aee1f8da8ef9f7296031d699\u0022)",
    "Label": {
      "encHashSHA1": "SHA-1"
    }
  },
  "hash.sha256": {
    "Key": "hash.sha256",
    "Method": "SHA-256",
    "Title": "SHA256 Hash Value Calculator Online",
    "Description": "SHA256 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022315f5bdb76d078c43b8ac0064e4a0164612b1fce77c869345bfc94c75894edd3\u0022)",
    "Label": {
      "encHashSHA256": "SHA-256"
    }
  },
  "hash.sha384": {
    "Key": "hash.sha384",
    "Method": "SHA-384",
    "Title": "SHA384 Hash Value Calculator Online",
    "Description": "SHA384 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u002255bc556b0d2fe0fce582ba5fe07baafff035653638c7ac0d5494c2a64c0bea1cc57331c7c12a45cdbca7f4c34a089eeb\u0022)",
    "Label": {
      "encHashSHA384": "SHA-384"
    }
  },
  "hash.sha512": {
    "Key": "hash.sha512",
    "Method": "SHA-512",
    "Title": "SHA512 Hash Value Calculator Online",
    "Description": "SHA512 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022c1527cd893c124773d811911970c8fe6e857d6df5dc9226bd8a160614c0cd963a4ddea2b94bb7d36021ef9d865d5cea294a82dd49a0bb269f51f6e7a57f79421\u0022)",
    "Label": {
      "encHashSHA512": "SHA-512"
    }
  },
  "hash.crc32": {
    "Key": "hash.crc32",
    "Method": "CRC32",
    "Title": "CRC32 Hash Value Calculator Online",
    "Description": "CRC32 hash value calculator.",
    "Tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022ebe6c6e6\u0022)",
    "Label": {
      "encHashCRC32": "CRC32"
    }
  }
}
""";
    }
}
