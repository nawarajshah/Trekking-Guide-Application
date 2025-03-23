import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuideItineraryListComponent } from './guide-itinerary-list.component';

describe('GuideItineraryListComponent', () => {
  let component: GuideItineraryListComponent;
  let fixture: ComponentFixture<GuideItineraryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuideItineraryListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuideItineraryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
