import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { postpetView } from "src/app/models/postpet.model";

@Component({
  selector: "app-postspet",
  templateUrl: "./postspet.component.html",
  styleUrls: ["./postspet.component.scss"],
})
export class PostspetComponent implements OnInit {
  constructor() {}

  @Output() loadMore = new EventEmitter();

  @Input() postspet: postpetView[] = [];
  @Input()
    set notMorePostPet(notMore: boolean){
      if(notMore){
        document.getElementById("load-button").style.display = "none"
      }
      else{
        document.getElementById("load-button").style.display = "block"
      }
    }

  ngOnInit(): void {}

  onLoadMore() {
    this.loadMore.emit("");
  }
}
