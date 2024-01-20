namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public static class Constants
    {
        public const string AllRequest =
"""
{
    "type": "all",
    "method": "all.all",
    "value": null,
    "oe": "UTF-8",
    "nl": "crlf",
    "tz": "UTC",
    "options": {}
}
""";

        public const string Methods =
"""
{
    "all.all": {
        "Key": "all.all",
        "useOe": true,
        "useNl": true,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": null,
        "title": "Encoding \u0026 Decoding Online Tools",
        "desc": "Encoding and Decoding site. e.g. HTML Escape / URL Encoding / Base64 / MD5 / SHA-1 / CRC32 / and many other String, Number, DateTime, Color, Hash formats!",
        "tooltip": "Enter the value to be converted. (e.g. String: \u0022Hello!\u0022 / Number: \u00221234.5\u0022 / Date: \u00221984-02-07T12:34:56\u0022 / Color: \u0022rgb(255 0 0)\u0022)",
        "label": {}
    },
    "string.all": {
        "Key": "string.all",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "String - ALL",
        "title": "String Encoder / Decoder, Converter Online",
        "desc": "String encoding and decoding converter. e.g. HTML Escape / URL Encoding / Quoted-printable / and many other formats!",
        "tooltip": "Enter the value to be converted. (e.g. \u0022Hello, world!\u0022)",
        "label": {}
    },
    "string.bin": {
        "Key": "string.bin",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Bin String",
        "title": "Bin to String Converter Online",
        "desc": "Bin to String converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002201001000 01100101 01101100 01101100 01101111 00101100 00100000 01110111 01101111 01110010 01101100 01100100 00100001\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002201001000 01100101 01101100 01101100 01101111 00101100 00100000 01110111 01101111 01110010 01101100 01100100 00100001\u0022)",
        "label": {
            "encStrBin": "Bin String",
            "decStrBin": "Bin String"
        }
    },
    "string.hex": {
        "Key": "string.hex",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Hex String",
        "title": "Hex to String Converter Online",
        "desc": "Hex to String converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002248 65 6C 6C 6F 2C 20 77 6F 72 6C 64 21\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u002248 65 6C 6C 6F 2C 20 77 6F 72 6C 64 21\u0022)",
        "label": {
            "encStrHex": "Hex String",
            "decStrHex": "Hex String"
        }
    },
    "string.html-escape": {
        "Key": "string.html-escape",
        "useOe": false,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "HTML Escape",
        "title": "HTML Escape / Unescape (Encoder / Decoder) Online",
        "desc": "HTML Escape / Unescape (encoder / decoder). (e.g. \u0022\u003Cp\u003EHello, world!\u003C/p\u003E\u0022 \u003C=\u003E \u0022\u0026lt;p\u0026gt;Hello, world!\u0026lt;/p\u0026gt;\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022\u003Cp\u003EHello, world!\u003C/p\u003E\u0022 \u003C=\u003E \u0022\u0026lt;p\u0026gt;Hello, world!\u0026lt;/p\u0026gt;\u0022)",
        "label": {
            "encStrHTMLEscape": "HTML Escape (Basic)",
            "encStrHTMLEscapeFully": "HTML Escape (Fully)",
            "decStrHTMLEscape": "HTML Escape"
        }
    },
    "string.url-encoding": {
        "Key": "string.url-encoding",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "URL Encoding",
        "title": "URL Encoding Encoder / Decoder Online",
        "desc": "URL Encoding encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022Hello%2C%20world%21\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022Hello%2C%20world%21\u0022)",
        "label": {
            "encStrURLEncoding": "URL Encoding",
            "decStrURLEncoding": "URL Encoding"
        }
    },
    "string.punycode": {
        "Key": "string.punycode",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Punycode IDN",
        "title": "Punycode IDN domain name Converter Online",
        "desc": "Punycode IDN domain name converter. (e.g. \u0022\u30C9\u30E1\u30A4\u30F3.com\u0022 \u003C=\u003E \u0022xn--eckwd4c7c.com\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022\u30C9\u30E1\u30A4\u30F3.com\u0022 \u003C=\u003E \u0022xn--eckwd4c7c.com\u0022)",
        "label": {
            "encStrPunycode": "Punycode IDN",
            "decStrPunycode": "Punycode IDN"
        }
    },
    "string.base32": {
        "Key": "string.base32",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Base32",
        "title": "Base32 Encoder / Decoder Online",
        "desc": "Base32 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022JBSWY3DPFQQHO33SNRSCC===\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022JBSWY3DPFQQHO33SNRSCC===\u0022)",
        "label": {
            "encStrBase32": "Base32",
            "decStrBase32": "Base32"
        }
    },
    "string.base45": {
        "Key": "string.base45",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Base45",
        "title": "Base45 Encoder / Decoder Online",
        "desc": "Base45 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022%69 VDK2EV4404ESVDX0\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022%69 VDK2EV4404ESVDX0\u0022)",
        "label": {
            "encStrBase45": "Base45",
            "decStrBase45": "Base45",
            "decStrBase45ZlibCoseCbor": "Base45/Zlib/COSE/CBOR"
        }
    },
    "string.base64": {
        "Key": "string.base64",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Base64",
        "title": "Base64 Encoder / Decoder Online",
        "desc": "Base64 encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022SGVsbG8sIHdvcmxkIQ==\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022SGVsbG8sIHdvcmxkIQ==\u0022)",
        "label": {
            "encStrBase64": "Base64",
            "decStrBase64": "Base64"
        }
    },
    "string.ascii85": {
        "Key": "string.ascii85",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Ascii85",
        "title": "Ascii85 Z85/Adobe/btoa Encoder / Decoder Online",
        "desc": "Ascii85 Z85/Adobe/btoa encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022nm=QNz.92Pz/PV8aP\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022nm=QNz.92Pz/PV8aP\u0022)",
        "label": {
            "encStrAscii85": "Ascii85",
            "decStrAscii85": "Ascii85"
        }
    },
    "string.quoted-printable": {
        "Key": "string.quoted-printable",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Quoted-printable",
        "title": "Quoted-printable Encoder / Decoder Online",
        "desc": "Quoted-printable encoder / decoder. (e.g. \u0022Hello, != world!\u0022 \u003C=\u003E \u0022Hello, !=3D world!\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, != world!\u0022 \u003C=\u003E \u0022Hello, !=3D world!\u0022)",
        "label": {
            "encStrQuotedPrintable": "Quoted-printable",
            "decStrQuotedPrintable": "Quoted-printable"
        }
    },
    "string.unicode-escape": {
        "Key": "string.unicode-escape",
        "useOe": false,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Unicode Escape",
        "title": "Unicode Escape (\\\\u %u \\\\x \u0026#x U\u002B 0x \\\\N) Encoder / Decoder Online",
        "desc": "Unicode Escape encoder / decoder. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\\\\u0048\\\\u0065\\\\u006c\\\\u006c\\\\u006f\\\\u002c\\\\u0020\\\\u0077\\\\u006f\\\\u0072\\\\u006c\\\\u0064\\\\u0021\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\\\\u0048\\\\u0065\\\\u006c\\\\u006c\\\\u006f\\\\u002c\\\\u0020\\\\u0077\\\\u006f\\\\u0072\\\\u006c\\\\u0064\\\\u0021\u0022)",
        "label": {
            "encStrUnicodeEscape": "Unicode Escape",
            "decStrUnicodeEscape": "Unicode Escape"
        }
    },
    "string.program-string": {
        "Key": "string.program-string",
        "useOe": false,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Program String",
        "title": "Program String Escape / Unescape Online",
        "desc": "Program String escape / unescape. (e.g. \u0022Hello,\tworld!\u0022 \u003C=\u003E \u0022\u0022Hello,\\\\tworld!\u0022\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello,\tworld!\u0022 \u003C=\u003E \u0022\u0022Hello,\\\\tworld!\u0022\u0022)",
        "label": {
            "encStrProgramString": "Program String",
            "decStrProgramString": "Program String"
        }
    },
    "string.morse-code": {
        "Key": "string.morse-code",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Morse Code",
        "title": "Morse Code Translator Online",
        "desc": "Morse Code translator. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022.... . .-.. .-.. --- --..-- / .-- --- .-. .-.. -.. -.-.--\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022.... . .-.. .-.. --- --..-- / .-- --- .-. .-.. -.. -.-.--\u0022)",
        "label": {
            "encStrMorseCode": "Morse Code",
            "decStrMorseCode": "Morse Code"
        }
    },
    "string.naming-convention": {
        "Key": "string.naming-convention",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Naming Convention",
        "title": "Camel Case / Snake Case / Kebab Case / Naming Convention Converter Online",
        "desc": "Camel Case / Snake Case / Kebab Case / Naming Convention converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022, \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022, \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022, \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022, \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
        "label": {}
    },
    "string.camel-case": {
        "Key": "string.camel-case",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Camel Case",
        "title": "Camel Case (Pascal Case) Converter Online",
        "desc": "Camel Case (Pascal Case) converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HelloWorld\u0022, \u0022helloWorld\u0022)",
        "label": {
            "encStrUpperCamelCase": "UpperCamelCase",
            "encStrLowerCamelCase": "lowerCamelCase"
        }
    },
    "string.snake-case": {
        "Key": "string.snake-case",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Snake Case",
        "title": "Snake Case Converter Online",
        "desc": "Snake Case converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO_WORLD\u0022, \u0022hello_world\u0022)",
        "label": {
            "encStrUpperSnakeCase": "UPPER_SNAKE_CASE",
            "encStrLowerSnakeCase": "lower_snake_case"
        }
    },
    "string.kebab-case": {
        "Key": "string.kebab-case",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Kebab Case",
        "title": "Kebab Case Converter Online",
        "desc": "Kebab Case converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO-WORLD\u0022, \u0022hello-world\u0022)",
        "label": {
            "encStrUpperKebabCase": "UPPER-KEBAB-CASE",
            "encStrLowerKebabCase": "lower-kebab-case"
        }
    },
    "string.character-width": {
        "Key": "string.character-width",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Character Width (Half, Full)",
        "title": "Character Width (Half-width, Full-width) Converter Online",
        "desc": "Character Width (Half-width, Full-width) converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\uFF28\uFF45\uFF4C\uFF4C\uFF4F\uFF0C\u3000\uFF57\uFF4F\uFF52\uFF4C\uFF44\uFF01\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022\uFF28\uFF45\uFF4C\uFF4C\uFF4F\uFF0C\u3000\uFF57\uFF4F\uFF52\uFF4C\uFF44\uFF01\u0022)",
        "label": {
            "encStrHalfWidth": "Half Width",
            "encStrFullWidth": "Full Width"
        }
    },
    "string.letter-case": {
        "Key": "string.letter-case",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Letter Case (Upper, Lower, Swap, Capital)",
        "title": "Letter Case (Upper-case, Lower-case) Converter Online",
        "desc": "Letter Case (Upper-case, Lower-case) converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO, WORLD!\u0022, \u0022hello, world!\u0022, \u0022hELLO, WORLD!\u0022, \u0022Hello, World!\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022HELLO, WORLD!\u0022, \u0022hello, world!\u0022, \u0022hELLO, WORLD!\u0022, \u0022Hello, World!\u0022)",
        "label": {
            "encStrUpperCase": "Upper Case",
            "encStrLowerCase": "Lower Case",
            "encStrSwapCase": "Swap Case",
            "encStrCapitalize": "Capitalize"
        }
    },
    "string.text-initials": {
        "Key": "string.text-initials",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Text Initials",
        "title": "Initials Text Converter Online",
        "desc": "Initials Text converter. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hw\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hw\u0022)",
        "label": {
            "encStrInitials": "Initials"
        }
    },
    "string.text-reverse": {
        "Key": "string.text-reverse",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Text Reverse",
        "title": "Reverse Text Converter Online",
        "desc": "Reverse Text converter. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022!dlrow ,olleH\u0022)",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 \u003C=\u003E \u0022!dlrow ,olleH\u0022)",
        "label": {
            "encStrReverse": "Reverse"
        }
    },
    "string.unicode-normalization": {
        "Key": "string.unicode-normalization",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Unicode Normalization",
        "title": "Unicode Normalization (NFC, NFKC, NFD, NFKD) Converter Online",
        "desc": "Unicode Normalization (NFC, NFKC, NFD, NFKD) converter.",
        "tooltip": "Enter the text to be converted.",
        "label": {
            "encStrUnicodeNFC": "Unicode NFC",
            "encStrUnicodeNFKC": "Unicode NFKC",
            "decStrUnicodeNFC": "Unicode NFD",
            "decStrUnicodeNFKC": "Unicode NFKD"
        }
    },
    "string.line-sort": {
        "Key": "string.line-sort",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Line Sort",
        "title": "Text Line Sorter (Asc, Desc, Reverse) Online",
        "desc": "Text Line Sorter (Asc, Desc, Reverse).",
        "tooltip": "Enter the multi-line text to be converted.",
        "label": {
            "encStrLineSort": "Line Sort"
        }
    },
    "string.line-unique": {
        "Key": "string.line-unique",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Line Unique",
        "title": "Text Line Duplicates Remover Online",
        "desc": "Text Line Duplicates Remover.",
        "tooltip": "Enter the multi-line text to be converted.",
        "label": {
            "encStrLineUnique": "Line Unique"
        }
    },
    "number.all": {
        "Key": "number.all",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Number - ALL",
        "title": "Number Converter Online",
        "desc": "Number converter. e.g. Binary numbers / Octal numbers / Decimal numbers / Hexadecimal numbers / English words numerals / Japanese numerals / and many other formats!",
        "tooltip": "Enter the number to be converted. (e.g. Dec: \u00221234.5\u0022 / Bin: \u002210011010010.1\u0022 / Oct: \u00222322.4\u0022 / Hex: \u00224d2.8\u0022 / EN: \u0022One Thousand Two Hundred Thirty-Four point Five\u0022 / JP: \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022)",
        "label": {}
    },
    "number.dec": {
        "Key": "number.dec",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Decimal Numbers",
        "title": "Decimal Numbers Converter Online",
        "desc": "Decimal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00221234.5\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00221234.5\u0022)",
        "label": {
            "encNumDec": "Num to Dec",
            "decNumDec": "Num from Dec"
        }
    },
    "number.bin": {
        "Key": "number.bin",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Binary Numbers",
        "title": "Binary Numbers Converter Online",
        "desc": "Binary numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u002210011010010.1\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u002210011010010.1\u0022)",
        "label": {
            "encNumBin": "Num to Bin",
            "decNumBin": "Num from Bin"
        }
    },
    "number.oct": {
        "Key": "number.oct",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Octal Numbers",
        "title": "Octal Numbers Converter Online",
        "desc": "Octal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00222322.4\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00222322.4\u0022)",
        "label": {
            "encNumOct": "Num to Oct",
            "decNumOct": "Num from Oct"
        }
    },
    "number.hex": {
        "Key": "number.hex",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Hexadecimal Numbers",
        "title": "Hexadecimal Numbers Converter Online",
        "desc": "Hexadecimal numbers converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u00224d2.8\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u00224d2.8\u0022)",
        "label": {
            "encNumHex": "Num to Hex",
            "decNumHex": "Num from Hex"
        }
    },
    "number.english": {
        "Key": "number.english",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "English Numerals",
        "title": "English Words Numerals Converter Online",
        "desc": "English words numerals converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u0022One Thousand Two Hundred Thirty-Four point Five\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u0022One Thousand Two Hundred Thirty-Four point Five\u0022)",
        "label": {
            "encNumEnShortScale": "Num to English",
            "decNumEnShortScale": "Num from English"
        }
    },
    "number.japanese": {
        "Key": "number.japanese",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Kanji Numerals",
        "title": "Japanese Kanji Numerals Converter Online",
        "desc": "Japanese Kanji numerals converter. e.g. \u00221234.5\u0022 \u003C=\u003E \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022",
        "tooltip": "Enter the number to be converted. (e.g. \u00221234.5\u0022 \u003C=\u003E \u0022\u5343\u4E8C\u767E\u4E09\u5341\u56DB\u30FB\u4E94\u0022, \u0022\u58F1\u9621\u5F10\u964C\u53C2\u62FE\u8086\u30FB\u4F0D\u0022)",
        "label": {
            "encNumJP": "Num to Kanji",
            "encNumJPDaiji": "Num to Kanji Daiji",
            "decNumJP": "Num from Kanji"
        }
    },
    "date.all": {
        "Key": "date.all",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Date - ALL",
        "title": "Date Time Converter Online",
        "desc": "Date Time converter. e.g. UNIX Time / ISO8601 Date / RFC2822 Date / and many other formats!",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022 / ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 / RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022)",
        "label": {}
    },
    "date.unix-time": {
        "Key": "date.unix-time",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "UNIX Time",
        "title": "UNIX Time (POSIX Time) Converter Online",
        "desc": "UNIX time (POSIX time) converter. e.g. ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E UNIX Time: \u0022444972896789\u0022",
        "tooltip": "Enter the date to be converted. (e.g. ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E UNIX Time: \u0022444972896789\u0022)",
        "label": {
            "encDateUnixTime": "UNIX Time [ms]"
        }
    },
    "date.w3cdtf": {
        "Key": "date.w3cdtf",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "W3C-DTF Date",
        "title": "W3C-DTF Date Converter Online",
        "desc": "W3C-DTF date converter. e.g. UNIX Time: \u0022444972896789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E W3C-DTF: \u00221984-02-07T12:34:56.789\u002B09:00\u0022",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E W3C-DTF: \u00221984-02-07T12:34:56.789\u002B09:00\u0022)",
        "label": {
            "encDateW3CDTF": "W3C-DTF Date"
        }
    },
    "date.iso8601": {
        "Key": "date.iso8601",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "ISO8601 Date",
        "title": "ISO8601 Date Converter Online",
        "desc": "ISO8601 date converter. e.g. UNIX Time: \u0022444972896789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022, RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022 =\u003E ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022)",
        "label": {
            "encDateISO8601": "ISO8601 Date",
            "encDateISO8601Ext": "ISO8601 Date (Extend)",
            "encDateISO8601Week": "ISO8601 Date (Week)",
            "encDateISO8601Ordinal": "ISO8601 Date (Ordinal)"
        }
    },
    "date.rfc2822": {
        "Key": "date.rfc2822",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "RFC2822 Date",
        "title": "RFC2822 Date Converter Online",
        "desc": "RFC2822 date converter. e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E RFC2822: \u0022Tue, 07 Feb 1984 12:34:56 JST\u0022)",
        "label": {
            "encDateRFC2822": "RFC2822 Date"
        }
    },
    "date.ctime": {
        "Key": "date.ctime",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "ctime Date",
        "title": "ctime Date Converter Online",
        "desc": "ctime date converter. e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E ctime: \u0022Tue Feb 07 12:34:56 1984\u0022",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E ctime: \u0022Tue Feb 07 12:34:56 1984\u0022)",
        "label": {
            "encDateCTime": "ctime Date"
        }
    },
    "date.japanese-era": {
        "Key": "date.japanese-era",
        "useOe": false,
        "useNl": false,
        "useTz": true,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Japanese Era",
        "title": "Japanese Era Date Converter Online",
        "desc": "Japanese Era date converter. e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E Japanese Era: \u0022\u662D\u548C59\u5E7402\u670807\u65E503\u664234\u520656.789\u79D2\u0022",
        "tooltip": "Enter the date to be converted. (e.g. UNIX Time: \u0022444972896789\u0022, ISO8601: \u00221984-02-07T12:34:56,789\u002B09:00\u0022 =\u003E Japanese Era: \u0022\u662D\u548C59\u5E7402\u670807\u65E503\u664234\u520656.789\u79D2\u0022)",
        "label": {
            "encDateJapaneseEra": "Japanese Era"
        }
    },
    "color.all": {
        "Key": "color.all",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Color - ALL",
        "title": "Color Converter Online",
        "desc": "Color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022)",
        "label": {}
    },
    "color.name": {
        "Key": "color.name",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Color Name",
        "title": "Color Name Converter Online",
        "desc": "Color name converter. e.g. \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022red\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022red\u0022)",
        "label": {
            "encColorName": "Color Name"
        }
    },
    "color.rgb": {
        "Key": "color.rgb",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "RGB Color",
        "title": "RGB Color Converter Online",
        "desc": "RGB color converter. e.g. \u0022red\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022)",
        "label": {
            "encColorRGBHex": "RGB Color (Hex)",
            "encColorRGBFn": "RGB Color"
        }
    },
    "color.hsl": {
        "Key": "color.hsl",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "HSL Color",
        "title": "HSL Color Converter Online",
        "desc": "HSL color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsl(0 100% 50%)\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsv(0 100% 100%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsl(0 100% 50%)\u0022)",
        "label": {
            "encColorHSLFn": "HSL Color"
        }
    },
    "color.hsv": {
        "Key": "color.hsv",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "HSV Color",
        "title": "HSV Color Converter Online",
        "desc": "HSV color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsv(0 100% 100%)\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022device-cmyk(0% 100% 100% 0%)\u0022 =\u003E \u0022hsv(0 100% 100%)\u0022)",
        "label": {
            "encColorHSVFn": "HSV Color"
        }
    },
    "color.cmyk": {
        "Key": "color.cmyk",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "CMYK Color",
        "title": "CMYK Color Converter Online",
        "desc": "CMYK color converter. e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022 =\u003E \u0022device-cmyk(0% 100% 100% 0%)\u0022",
        "tooltip": "Enter the color to be converted. (e.g. \u0022red\u0022, \u0022#ff0000\u0022, \u0022rgb(255 0 0)\u0022, \u0022hsl(0 100% 50%)\u0022, \u0022hsv(0 100% 100%)\u0022 =\u003E \u0022device-cmyk(0% 100% 100% 0%)\u0022)",
        "label": {
            "encColorCMYKFn": "CMYK Color"
        }
    },
    "cipher.all": {
        "Key": "cipher.all",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Cipher - ALL",
        "title": "Cipher Encrypter / Decrypter Online",
        "desc": "Cipher encrypter / decrypter. e.g. Caesar / ROT / and many other formats!",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022)",
        "label": {}
    },
    "cipher.caesar": {
        "Key": "cipher.caesar",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Caesar Cipher",
        "title": "Caesar Cipher Encrypter / Decrypter Online",
        "desc": "Caesar Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Ebiil, tloia!\u0022)",
        "label": {
            "encCipherCaesar": "Caesar",
            "decCipherCaesar": "Caesar"
        }
    },
    "cipher.rot13": {
        "Key": "cipher.rot13",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "ROT13",
        "title": "ROT13 Encrypter / Decrypter Online",
        "desc": "ROT13 encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022Uryyb, jbeyq 123!\u0022)",
        "label": {
            "encCipherROT13": "ROT13 (A-Z)",
            "decCipherROT13": "ROT13 (A-Z)"
        }
    },
    "cipher.rot18": {
        "Key": "cipher.rot18",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "ROT18",
        "title": "ROT18 Encrypter / Decrypter Online",
        "desc": "ROT18 encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022Uryyb, jbeyq 678!\u0022)",
        "label": {
            "encCipherROT18": "ROT18 (A-Z, 0-9)",
            "decCipherROT18": "ROT18 (A-Z, 0-9)"
        }
    },
    "cipher.rot47": {
        "Key": "cipher.rot47",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "ROT47",
        "title": "ROT47 Encrypter / Decrypter Online",
        "desc": "ROT47 encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world 123!\u0022 =\u003E \u0022w6==@[ H@C=5 \u0060abP\u0022)",
        "label": {
            "encCipherROT47": "ROT47 (!-~)",
            "decCipherROT47": "ROT47 (!-~)"
        }
    },
    "cipher.atbash": {
        "Key": "cipher.atbash",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Atbash",
        "title": "Atbash Cipher Encrypter / Decrypter Online",
        "desc": "Atbash Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Svool, dliow!\u0022)",
        "label": {
            "encCipherAtbash": "Atbash"
        }
    },
    "cipher.affine": {
        "Key": "cipher.affine",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Affine",
        "title": "Affine Cipher Encrypter / Decrypter Online",
        "desc": "Affine Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Ebiil, tloia!\u0022)",
        "label": {
            "encCipherAffine": "Affine",
            "decCipherAffine": "Affine"
        }
    },
    "cipher.vigenere": {
        "Key": "cipher.vigenere",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Vigen\u00E8re",
        "title": "Vigen\u00E8re Cipher Encrypter / Decrypter Online",
        "desc": "Vigen\u00E8re Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Zincs, aqipw!\u0022)",
        "label": {
            "encCipherVigenere": "Vigen\u00E8re",
            "decCipherVigenere": "Vigen\u00E8re"
        }
    },
    "cipher.enigma": {
        "Key": "cipher.enigma",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Enigma",
        "title": "Enigma Machine Simulator Online",
        "desc": "Enigma Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Kcubr, kidkn!\u0022)",
        "label": {
            "encCipherEnigma": "Enigma",
            "decCipherEnigma": "Enigma"
        }
    },
    "cipher.jis-keyboard": {
        "Key": "cipher.jis-keyboard",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "JIS Keyboard",
        "title": "JIS Keyboard Encrypter / Decrypter Online",
        "desc": "JIS Keyboard encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022ntt\u0022 =\u003E \u0022\u307F\u304B\u304B\u0022)",
        "label": {
            "encCipherJisKeyboard": "JIS Keyboard"
        }
    },
    "cipher.scytale": {
        "Key": "cipher.scytale",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Scytale Cipher",
        "title": "Scytale Cipher Encrypter / Decrypter Online",
        "desc": "Scytale Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hweolrllod,! \u0022)",
        "label": {
            "encCipherScytale": "Scytale",
            "decCipherScytale": "Scytale"
        }
    },
    "cipher.rail-fence": {
        "Key": "cipher.rail-fence",
        "useOe": false,
        "useNl": false,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": true,
        "method": "Rail Fence Cipher",
        "title": "Rail Fence Cipher Encrypter / Decrypter Online",
        "desc": "Rail Fence Cipher encrypter / decrypter.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022Hlo ol!el,wrd\u0022)",
        "label": {
            "encCipherRailFence": "Rail Fence",
            "decCipherRailFence": "Rail Fence"
        }
    },
    "hash.all": {
        "Key": "hash.all",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "Hash - ALL",
        "title": "Hash Value Calculator Online",
        "desc": "Hash value calculator. e.g. MD2 / MD5 / SHA-1 / SHA-256 / SHA-384 / SHA-512 / CRC32 / and many other formats!",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022)",
        "label": {}
    },
    "hash.md2": {
        "Key": "hash.md2",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "MD2",
        "title": "MD2 Hash Value Calculator Online",
        "desc": "MD2 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u00228cca0e965edd0e223b744f9cedf8e141\u0022)",
        "label": {
            "encHashMD2": "MD2"
        }
    },
    "hash.md5": {
        "Key": "hash.md5",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "MD5",
        "title": "MD5 Hash Value Calculator Online",
        "desc": "MD5 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u00226cd3556deb0da54bca060b4c39479839\u0022)",
        "label": {
            "encHashMD5": "MD5"
        }
    },
    "hash.sha1": {
        "Key": "hash.sha1",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "SHA-1",
        "title": "SHA1 Hash Value Calculator Online",
        "desc": "SHA1 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022943a702d06f34599aee1f8da8ef9f7296031d699\u0022)",
        "label": {
            "encHashSHA1": "SHA-1"
        }
    },
    "hash.sha256": {
        "Key": "hash.sha256",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "SHA-256",
        "title": "SHA256 Hash Value Calculator Online",
        "desc": "SHA256 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022315f5bdb76d078c43b8ac0064e4a0164612b1fce77c869345bfc94c75894edd3\u0022)",
        "label": {
            "encHashSHA256": "SHA-256"
        }
    },
    "hash.sha384": {
        "Key": "hash.sha384",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "SHA-384",
        "title": "SHA384 Hash Value Calculator Online",
        "desc": "SHA384 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u002255bc556b0d2fe0fce582ba5fe07baafff035653638c7ac0d5494c2a64c0bea1cc57331c7c12a45cdbca7f4c34a089eeb\u0022)",
        "label": {
            "encHashSHA384": "SHA-384"
        }
    },
    "hash.sha512": {
        "Key": "hash.sha512",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "SHA-512",
        "title": "SHA512 Hash Value Calculator Online",
        "desc": "SHA512 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022c1527cd893c124773d811911970c8fe6e857d6df5dc9226bd8a160614c0cd963a4ddea2b94bb7d36021ef9d865d5cea294a82dd49a0bb269f51f6e7a57f79421\u0022)",
        "label": {
            "encHashSHA512": "SHA-512"
        }
    },
    "hash.crc32": {
        "Key": "hash.crc32",
        "useOe": true,
        "useNl": true,
        "useTz": false,
        "hasEncoded": true,
        "hasDecoded": false,
        "method": "CRC32",
        "title": "CRC32 Hash Value Calculator Online",
        "desc": "CRC32 hash value calculator.",
        "tooltip": "Enter the text to be converted. (e.g. \u0022Hello, world!\u0022 =\u003E \u0022ebe6c6e6\u0022)",
        "label": {
            "encHashCRC32": "CRC32"
        }
    }
}
""";
    }
}
