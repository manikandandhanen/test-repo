import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DateOfJoiningInfoComponent } from './date-of-joining-info.component';

describe('DateOfJoiningInfoComponent', () => {
  let component: DateOfJoiningInfoComponent;
  let fixture: ComponentFixture<DateOfJoiningInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DateOfJoiningInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DateOfJoiningInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
