using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class DenCodeClientTests
    {
        private DenCodeClient subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "http://localhost/dencode")
                .WithPartialContent("all.all")
                .Respond("application/json", "{\"statusCode\":200,\"messages\":[],\"redirectUrl\":null,\"response\":{\"encDateRFC2822\":null,\"encDateISO8601\":null,\"encDateISO8601Week\":null,\"encDateISO8601Ordinal\":null,\"encDateISO8601Ext\":null,\"encDateW3CDTF\":null,\"encDateCTime\":null,\"encDateJapaneseEra\":null,\"encDateUnixTime\":null,\"encStrPunycode\":\"value\",\"decStrPunycode\":\"value\",\"encStrLineSort\":\"value\",\"encStrUnicodeNFC\":\"value\",\"encStrUnicodeNFKC\":\"value\",\"decStrUnicodeNFC\":\"value\",\"decStrUnicodeNFKC\":\"value\",\"encStrLineUnique\":\"value\",\"encStrProgramString\":\"\\\"value\\\"\",\"decStrProgramString\":\"value\",\"encStrQuotedPrintable\":\"value\",\"decStrQuotedPrintable\":\"value\",\"encStrHex\":\"76616c7565\",\"decStrHex\":null,\"encStrMorseCode\":\"...- .- .-.. ..- .\",\"decStrMorseCode\":\"value\",\"encStrBase32\":\"OZQWY5LF\",\"decStrBase32\":\"�\\u0017B\",\"encStrUpperCase\":\"VALUE\",\"encStrLowerCase\":\"value\",\"encStrSwapCase\":\"VALUE\",\"encStrCapitalize\":\"Value\",\"encStrReverse\":\"eulav\",\"encStrUnicodeEscape\":\"\\\\u0076\\\\u0061\\\\u006c\\\\u0075\\\\u0065\",\"decStrUnicodeEscape\":\"value\",\"encStrBase45\":\"K/E0WDB2\",\"decStrBase45\":null,\"decStrBase45ZlibCoseCbor\":null,\"encStrAscii85\":\"C42ifwD\",\"decStrAscii85\":\"`�!b\",\"encStrHalfWidth\":\"value\",\"encStrFullWidth\":\"ｖａｌｕｅ\",\"encStrURLEncoding\":\"value\",\"decStrURLEncoding\":\"value\",\"encStrBin\":\"0111011001100001011011000111010101100101\",\"decStrBin\":null,\"encStrUpperCamelCase\":\"Value\",\"encStrLowerCamelCase\":\"value\",\"encStrInitials\":\"v\",\"encStrUpperSnakeCase\":\"VALUE\",\"encStrLowerSnakeCase\":\"value\",\"encStrUpperKebabCase\":\"VALUE\",\"encStrLowerKebabCase\":\"value\",\"encStrHTMLEscape\":\"value\",\"encStrHTMLEscapeFully\":\"&#x76;&#x61;&#x6c;&#x75;&#x65;\",\"decStrHTMLEscape\":\"value\",\"encStrBase64\":\"dmFsdWU=\",\"decStrBase64\":\"��n\",\"encCipherEnigma\":\"tsunt\",\"decCipherEnigma\":\"tsunt\",\"encCipherROT47\":\"G2=F6\",\"decCipherROT47\":\"G2=F6\",\"encCipherROT13\":\"inyhr\",\"decCipherROT13\":\"inyhr\",\"encCipherScytale\":\"vuael\",\"decCipherScytale\":\"vleau\",\"encCipherROT18\":\"inyhr\",\"decCipherROT18\":\"inyhr\",\"encCipherJisKeyboard\":\"ひちりない\",\"encCipherCaesar\":\"sxirb\",\"decCipherCaesar\":\"ydoxh\",\"encCipherRailFence\":\"vleau\",\"decCipherRailFence\":\"vuael\",\"encNumOct\":null,\"decNumOct\":null,\"encNumBin\":null,\"decNumBin\":null,\"encNumJP\":null,\"encNumJPDaiji\":null,\"decNumJP\":null,\"encNumEnShortScale\":null,\"decNumEnShortScale\":null,\"encNumDec\":null,\"decNumDec\":null,\"encNumHex\":null,\"decNumHex\":null,\"encColorRGBHex\":null,\"encColorRGBFn\":null,\"encColorCMYKFn\":null,\"encColorHSVFn\":null,\"encColorName\":null,\"encColorHSLFn\":null,\"encHashCRC32\":\"1d775834\",\"encHashSHA384\":\"b46c7c39e15d3dc2cdc50e42a7a28181a074ceef18c7c4aa2f6d987fb80e16593839c6aecf4fe38f879ce14e6dfd08c5\",\"encHashSHA1\":\"f32b67c7e26342af42efabc674d441dca0a281c5\",\"encHashSHA256\":\"cd42404d52ad55ccfa9aca4adc828aa5800ad9d385a0671fbcbf724118320619\",\"encHashMD2\":\"b8f02101126e15407212115c2b076d83\",\"encHashSHA512\":\"ec2c83edecb60304d154ebdb85bdfaf61a92bd142e71c4f7b25a15b9cb5f3c0ae301cfb3569cf240e4470031385348bc296d8d99d09e06b26f09591a97527296\",\"encHashMD5\":\"2063c1608d6e0baf80249c42e2be5804\",\"textLength\":5,\"textByteLength\":5}}");
            mockHttp.When(HttpMethod.Post, "http://localhost/dencode")
                .WithPartialContent("hash.crc32")
                .Respond("application/json", "{\"statusCode\":200,\"messages\":[],\"redirectUrl\":null,\"response\":{\"encHashCRC32\":\"1d775834\",\"textLength\":5,\"textByteLength\":5}}");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            subject = new DenCodeClient(httpClient);
        }

        [TestMethod]
        public async Task DenCodeAsync()
        {
            var result = await subject.DenCodeAsync("value");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task DenCodeAsync_method()
        {
            var method = new DenCodeMethod
            {
                Key = "hash.crc32"
            };
            var result = await subject.DenCodeAsync(method, "value");
            result.Should().NotBeNull();
        }
    }
}
