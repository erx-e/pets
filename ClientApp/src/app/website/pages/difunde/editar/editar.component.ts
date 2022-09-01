import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-editarDifunde',
  template: ` <app-editar [stateId]="stateId"></app-editar>`,
})
export class EditarComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  stateId = "B"
}
