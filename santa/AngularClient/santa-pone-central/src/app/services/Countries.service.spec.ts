/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CountriesService } from './countries.service';

describe('Service: Countries', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CountriesService]
    });
  });

  it('should ...', inject([CountriesService], (service: CountriesService) => {
    expect(service).toBeTruthy();
  }));
});
