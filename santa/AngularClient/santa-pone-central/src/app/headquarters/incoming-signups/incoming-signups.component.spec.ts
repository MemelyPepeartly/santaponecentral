import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingSignupsComponent } from './incoming-signups.component';

describe('IncomingSignupsComponent', () => {
  let component: IncomingSignupsComponent;
  let fixture: ComponentFixture<IncomingSignupsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingSignupsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingSignupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
