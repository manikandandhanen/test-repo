import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobHistoryInfoComponent } from './job-history-info.component';

describe('JobHistoryInfoComponent', () => {
  let component: JobHistoryInfoComponent;
  let fixture: ComponentFixture<JobHistoryInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobHistoryInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobHistoryInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
