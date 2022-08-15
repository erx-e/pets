import { Component, OnInit, Input } from '@angular/core';
import { postpetView } from 'src/app/models/postpet.model';

@Component({
  selector: 'app-postpet',
  templateUrl: './postpet.component.html',
  styleUrls: ['./postpet.component.scss']
})
export class PostpetComponent implements OnInit {

  constructor() { }

  @Input() postpet: postpetView | null;

  ngOnInit(): void {
  }

}
