import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaceEditComponent } from './place-edit.component';

describe('PlaceEditComponent', () => {
  let component: PlaceEditComponent;
  let fixture: ComponentFixture<PlaceEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlaceEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaceEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
