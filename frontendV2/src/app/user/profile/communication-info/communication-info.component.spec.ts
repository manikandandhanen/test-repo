import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationInfoComponent } from './communication-info.component';

describe('CommunicationInfoComponent', () => {
  let component: CommunicationInfoComponent;
  let fixture: ComponentFixture<CommunicationInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommunicationInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommunicationInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
