"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WordFormsService = void 0;
var WordFormsService = /** @class */ (function () {
    function WordFormsService(http) {
        this.http = http;
    }
    WordFormsService.prototype.search = function (search) {
        return this.http.get('word-forms/search', { params: { q: search } });
    };
    return WordFormsService;
}());
exports.WordFormsService = WordFormsService;
//# sourceMappingURL=word-forms-service.js.map