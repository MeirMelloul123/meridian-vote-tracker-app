import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Imports } from './imports';

describe('Imports', () => {
  let component: Imports;
  let fixture: ComponentFixture<Imports>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Imports],
    }).compileComponents();

    fixture = TestBed.createComponent(Imports);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
