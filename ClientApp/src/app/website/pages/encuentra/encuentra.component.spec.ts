import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EncuentraComponent } from './encuentra.component';

describe('EncuentraComponent', () => {
  let component: EncuentraComponent;
  let fixture: ComponentFixture<EncuentraComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EncuentraComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EncuentraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
