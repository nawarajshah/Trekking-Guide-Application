import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RequestService } from '../../service/request.service';
import { ToastrService } from 'ngx-toastr';
import { Request } from '../../service/request.service';

@Component({
  selector: 'app-my-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-requests.component.html',
  styleUrl: './my-requests.component.css'
})
export class MyRequestsComponent implements OnInit {
  requests: Request[] = [];

  constructor(
    private requestService: RequestService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.requestService.getMyRequests().subscribe({
      next: (data) => {
        this.requests = data;
      },
      error: (err) => {
        this.toastr.error("Error loading your requests");
        console.error("Error loading: " + err);
      }
    });
  }
}
