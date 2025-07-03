import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ContactComponent } from './contact/contact';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ContactComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'angular-Farmtrade';
}
