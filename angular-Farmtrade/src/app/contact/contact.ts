import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.html',
  styleUrls: ['./contact.css']
})
export class ContactComponent {
  name = '';
  email = '';
  message = '';

  submitForm() {
    alert(`Name: ${this.name}\nEmail: ${this.email}\nMessage: ${this.message}`);
  }
}
