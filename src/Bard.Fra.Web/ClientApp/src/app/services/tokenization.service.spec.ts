import { TestBed } from '@angular/core/testing';

import { TokenizationService } from './tokenization.service';

describe('TokenizationService', () => {
  let service: TokenizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenizationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
