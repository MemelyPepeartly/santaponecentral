/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SantaApiServiceService } from './SantaApiService.service';

describe('Service: SantaApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SantaApiServiceService]
    });
  });

  it('should ...', inject([SantaApiServiceService], (service: SantaApiServiceService) => {
    expect(service).toBeTruthy();
  }));
});
