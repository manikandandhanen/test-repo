import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DependentInfoComponent } from './dependent-info.component';

describe('DependentInfoComponent', () => {
  let component: DependentInfoComponent;
  let fixture: ComponentFixture<DependentInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DependentInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DependentInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
