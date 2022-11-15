import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EductionalInfoComponent } from './eductional-info.component';

describe('EductionalInfoComponent', () => {
  let component: EductionalInfoComponent;
  let fixture: ComponentFixture<EductionalInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EductionalInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EductionalInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
