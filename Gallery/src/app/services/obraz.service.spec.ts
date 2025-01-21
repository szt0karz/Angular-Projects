import { TestBed } from '@angular/core/testing';

import { ObrazService } from './obraz.service';

describe('ObrazService', () => {
  let service: ObrazService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ObrazService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
