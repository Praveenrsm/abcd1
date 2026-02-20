import { Component,OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app-service';
import { AppEntity } from './app-entity';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ CommonModule ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  users:AppEntity[]=[];
  constructor(private app:AppService, private cdr: ChangeDetectorRef) { 
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(): void {
  this.app.getAllUsers().subscribe({
    next: (data) => {
      this.users = data;
      console.log("Users fetched:", this.users);
      this.cdr.detectChanges();
    },
    error: (err) => {
      console.error("API error:", err);
    }
  });
}
}