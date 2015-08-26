/*http://bitwiseshiftleft.github.io/sjcl/doc/
    

*/
describe("Test Encryption PublicKey", function () {
    var testResult = noFuture.host.shell.encryption.PublicKey.ToEncryptedText("public key plain test", "wEk*wnGz'6");

    it("should be some kind of cipher text", function () {
        expect(testResult).toBeDefined();
        console.log("PK encrypted text: " + testResult);
    });
});

describe("Test Encryption BulkCipher", function () {
    var testKey = ".G%rs?26YT";
    var testText = "public key plain text";
    var testResultIn = noFuture.host.shell.encryption.BulkCipher.ToEncryptedText(testText, testKey);
    var testResultOut = noFuture.host.shell.encryption.BulkCipher.ToPlainText(testResultIn, testKey);

    it("should be some kind of cipher text", function () {
        expect(testResultIn).toBeDefined();
        console.log("BC encrypted text: " + testResultIn);
    });
    it("should be some kind of plain text", function () {
        expect(testResultOut).toBeDefined();
        console.log("BC decrypted text: " + testResultOut);
    });

    it("should have toggled plain-to-encrypted-to-plain text without loss", function () {
        expect(testResultOut).toBe(testText);
    });

    //{"iv":"3iNrgjV1EeW8gg8wfzLvHw==","v":1,"iter":1000,"ks":128,"ts":64,"mode":"ccm","adata":"","cipher":"aes","salt":"ZhB5LSZujLM=","ct":"5bEavZH9A+vvSz6DYkU4XftUJ9DYB0os1leaamI="}
    //var sjclJsonDecrypt = sjcl.json.decrypt(testKey, { "iv": "3iNrgjV1EeW8gg8wfzLvHw==", "v": 1, "iter": 1000, "ks": 128, "ts": 64, "mode": "ccm", "adata": "", "cipher": "aes", "salt": "ZhB5LSZujLM=", "ct": "5bEavZH9A+vvSz6DYkU4XftUJ9DYB0os1leaamI=" });
});

describe("Test Encryption Hash", function () {
    var testResult = noFuture.host.shell.encryption.Hash.Sign("plain text", "oWu,2zSFOO");

    it("should be some kind of hash", function () {
        expect(testResult).toBeDefined();
        console.log("Hash result: " + testResult);
    });
});