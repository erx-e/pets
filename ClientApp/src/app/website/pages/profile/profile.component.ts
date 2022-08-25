import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import { postpetView } from "src/app/models/postpet.model";
import { UserView } from "src/app/models/user.model";
import { AuthService } from "src/app/services/auth.service";
import { LoadingService } from "src/app/services/loading.service";
import { PostpetService } from "src/app/services/postpet.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"],
})
export class ProfileComponent implements OnInit {
  constructor(
    private router: ActivatedRoute,
    private postpetService: PostpetService,
    private authService: AuthService,
    private loadingService: LoadingService
  ) {}

  userId: number = null;
  user: UserView = null;
  postspet: postpetView[] = [];
  isLoading: boolean = true;
  loadingSubscription: Subscription

  ngOnInit(): void {
    this.loadingSubscription = this.loadingService.isLoading$.subscribe(data => this.isLoading = data)
    this.router.paramMap.subscribe((params) => {
      if (params.get("id")) {
        this.userId = parseInt(params.get("id"));
        if (this.userId != NaN) {
          this.authService
            .getProfile(this.userId)
            .subscribe((data) => (this.user = data));
        }
      }
    });
    this.loadingSubscription.unsubscribe()
    this.postpetService.GetByFilter(
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      this.userId
    ).subscribe(data => {
      this.postspet = data;
    });
  }
}
