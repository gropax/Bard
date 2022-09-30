"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Maybe = exports.Paginated = exports.PaginationParams = void 0;
var PaginationParams = /** @class */ (function () {
    function PaginationParams(page, pageSize, sort, sortDir) {
        this.page = page;
        this.pageSize = pageSize;
        this.sort = sort;
        this.sortDir = sortDir;
    }
    Object.defineProperty(PaginationParams.prototype, "startIndex", {
        get: function () { return (this.page - 1) * this.pageSize; },
        enumerable: false,
        configurable: true
    });
    PaginationParams.prototype.toStringParams = function () {
        return {
            page: this.page.toString(),
            pageSize: this.pageSize.toString(),
            sortColumn: this.sort.toString(),
            sortDirection: this.sortDir.toString(),
        };
    };
    return PaginationParams;
}());
exports.PaginationParams = PaginationParams;
var Paginated = /** @class */ (function () {
    function Paginated(page, pageSize, sort, sortDir, total, items) {
        this.page = page;
        this.pageSize = pageSize;
        this.sort = sort;
        this.sortDir = sortDir;
        this.total = total;
        this.items = items;
    }
    return Paginated;
}());
exports.Paginated = Paginated;
var Maybe = /** @class */ (function () {
    function Maybe(hasValue, value) {
        this.hasValue = hasValue;
        this.value = value;
    }
    return Maybe;
}());
exports.Maybe = Maybe;
//# sourceMappingURL=paginated.js.map