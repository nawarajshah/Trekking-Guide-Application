import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuideItineraryDetailsComponent } from './guide-itinerary-details.component';

describe('GuideItineraryDetailsComponent', () => {
  let component: GuideItineraryDetailsComponent;
  let fixture: ComponentFixture<GuideItineraryDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuideItineraryDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuideItineraryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
