import { Component, OnInit } from '@angular/core';
import { Sala } from '../../models/sala.model'; 
import { SalaService } from '../../services/sala.service';

@Component({
  selector: 'app-sala-list',
  templateUrl: './sala-list.component.html',
})
export class SalaListComponent implements OnInit {
  
  Sala: Sala[] = [];

  constructor(private SalaService: SalaService) { }

  ngOnInit(): void {
    this.SalaService.ottieniTutto().subscribe(data => {
      this.Sala = data;
    });
  }
}