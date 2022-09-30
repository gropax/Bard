"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.RhymingWordDataSource = void 0;
var rxjs_1 = require("rxjs");
var rxjs_2 = require("rxjs");
var rxjs_3 = require("rxjs");
/*
 * Tutorial on DataSource: https://blog.angular-university.io/angular-material-data-table/
 */
var RhymingWordDataSource = /** @class */ (function () {
    function RhymingWordDataSource(graphService) {
        this.graphService = graphService;
        this.finalRhymeSubject = new rxjs_3.BehaviorSubject([]);
        this.loadingSubject = new rxjs_3.BehaviorSubject(false);
        this.loading$ = this.loadingSubject.asObservable();
    }
    RhymingWordDataSource.prototype.connect = function (collectionViewer) {
        return this.finalRhymeSubject.asObservable();
    };
    RhymingWordDataSource.prototype.disconnect = function (collectionViewer) {
        this.finalRhymeSubject.complete();
        this.loadingSubject.complete();
    };
    RhymingWordDataSource.prototype.loadLessons = function (graphemes, phonemes, filter, sortDirection, pageIndex, pageSize) {
        var _this = this;
        if (filter === void 0) { filter = ''; }
        if (sortDirection === void 0) { sortDirection = 'asc'; }
        if (pageIndex === void 0) { pageIndex = 0; }
        if (pageSize === void 0) { pageSize = 10; }
        this.loadingSubject.next(true);
        this.graphService
            .getRhymingWords(graphemes, phonemes, filter, sortDirection, pageIndex, pageSize)
            .pipe(rxjs_2.map(function (p) { return p.items; }), rxjs_1.catchError(function () { return rxjs_1.of([]); }), rxjs_1.finalize(function () { return _this.loadingSubject.next(false); }))
            .subscribe(function (rhymes) { return _this.finalRhymeSubject.next(rhymes); });
    };
    return RhymingWordDataSource;
}());
exports.RhymingWordDataSource = RhymingWordDataSource;
//# sourceMappingURL=final-rhyme-data-source.js.map