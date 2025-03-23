import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditItineraryComponent } from './add-edit-itinerary.component';

describe('AddEditItineraryComponent', () => {
  let component: AddEditItineraryComponent;
  let fixture: ComponentFixture<AddEditItineraryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddEditItineraryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditItineraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
