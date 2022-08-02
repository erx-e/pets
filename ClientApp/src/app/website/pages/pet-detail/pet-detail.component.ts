import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { switchMap } from "rxjs/operators";
import { postpetView } from "src/app/models/postpet.model";
import { UserView } from "src/app/models/user.model";
import { PostpetService } from "src/app/services/postpet.service";
import Swiper, { Navigation, Pagination, SwiperOptions } from "swiper";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: "app-pet-detail",
  templateUrl: "./pet-detail.component.html",
  styleUrls: ["./pet-detail.component.scss"],
})
export class PetDetailComponent implements OnInit {
  constructor(
    private router: ActivatedRoute,
    private postpetService: PostpetService,
    private location: Location,
    private authService: AuthService
  ) {}

  postpetId: string | null = null;
  postpet: postpetView | null = null;
  user: UserView | null;

  swiperConfig: SwiperOptions = {
    pagination: true,
    navigation: true,
    slidesPerView: 1,
    spaceBetween: 25,
  }

  ngOnInit(): void {
    this.authService.user$.subscribe((data) => (this.user = data));

    this.router.paramMap
      .pipe(
        switchMap((params) => {
          this.postpetId = params.get("id");
          if (this.postpetId) {
            return this.postpetService.getById(parseInt(this.postpetId));
          }
          return null;
        })
      )
      .subscribe((data) => (this.postpet = data));

    Swiper.use([Pagination, Navigation])
  }

  deletePost(){
    this.postpetService.delete(parseInt(this.postpet.id)).subscribe(()=> {
      this.postpet.urlImgs.
      this.postpetService.deleteImg()
      this.location.back()
    })
  }

  goToBack() {
    this.location.back();
  }
}
