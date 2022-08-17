import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { switchMap } from "rxjs/operators";
import { postpetView } from "src/app/models/postpet.model";
import { UserView } from "src/app/models/user.model";
import { PostpetService } from "src/app/services/postpet.service";
import Swiper, { Navigation, Pagination, SwiperOptions } from "swiper";
import { AuthService } from "src/app/services/auth.service";
import { of } from "rxjs";
import format from "date-fns/format";
import es from "date-fns/locale/es";
import { LoadingService } from "src/app/services/loading.service";

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
    private authService: AuthService,
    private loadingService: LoadingService
  ) {}

  postpetId: string | null = null;
  postpet: postpetView | null = null;
  user: UserView | null;
  lastTimeSeen: string = ""
  notFound: boolean = false;
  isLoading: boolean = true;



  swiperConfig: SwiperOptions = {
    pagination: true,
    navigation: true,
    slidesPerView: 1,
    spaceBetween: 25,
  };

   ngOnInit(): void {
    this.authService.user$.subscribe((data) => (this.user = data));
    this.loadingService.isLoading$.subscribe(isL => this.isLoading = isL)
     this.router.paramMap
      .pipe(
        switchMap((params) => {
          this.postpetId = params.get("id");
          if (this.postpetId) {
            return this.postpetService.getById(parseInt(this.postpetId));
          }
          else{
            
          }
          return of(null);
        })
      )
      .subscribe((data) => {
        if (data) {
          this.postpet = data;
          this.lastTimeSeen = format(new Date(this.postpet.lastTimeSeen), "dd 'de' MMMM 'del' yyyy 'a las' HH:mm", {locale: es})
        }
      });

    Swiper.use([Pagination, Navigation]);



  }

  deletePost() {
    this.postpetService.delete(parseInt(this.postpet.id)).subscribe(() => {
      this.postpet.urlImgs.forEach((url) => {
        let key = url.url.split("/").pop();
        this.postpetService.deleteImg(key).then(() => {console.log(url.url)});
      });
      this.location.back();
    });
  }

  goToBack() {
    this.location.back();
  }
}
