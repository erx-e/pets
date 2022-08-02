import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DifundeComponent } from './difunde.component';

describe('DifundeComponent', () => {
  let component: DifundeComponent;
  let fixture: ComponentFixture<DifundeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DifundeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DifundeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
