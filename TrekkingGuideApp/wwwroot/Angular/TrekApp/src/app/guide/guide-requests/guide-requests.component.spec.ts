import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuideRequestsComponent } from './guide-requests.component';

describe('GuideRequestsComponent', () => {
  let component: GuideRequestsComponent;
  let fixture: ComponentFixture<GuideRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuideRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuideRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
