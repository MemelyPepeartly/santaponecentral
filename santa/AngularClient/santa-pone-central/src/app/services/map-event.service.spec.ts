/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MapEventService } from './map-event.service';

describe('Service: MapEvent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MapEventService]
    });
  });

  it('should ...', inject([MapEventService], (service: MapEventService) => {
    expect(service).toBeTruthy();
  }));
});
