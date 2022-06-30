import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CreateUserDTO } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private userService: UserService ) { }

  ngOnInit(): void {
  }

  user: CreateUserDTO;

  OnRegister(user: CreateUserDTO){
    this.userService.create(user);
  }
}
