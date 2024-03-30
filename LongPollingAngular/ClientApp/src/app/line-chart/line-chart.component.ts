import { Component } from '@angular/core';
import Chart from 'chart.js/auto';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent {
  public chart: any;
  DATE_TIME_FORMAT: string = 'HH:mm'; 
constructor(private readonly simpleService: MessageService){

}


  ngOnInit(): void {
    this.createChart();
    this.simpleService.count$.subscribe((data) => {
      this.log(data.count);
      this.chart.data.labels.push(new Date().toLocaleTimeString());
      this.chart.data.label = data.name;
      this.chart.data.datasets.forEach((dataset:any)=>{
          dataset.data.push(data.count);
          this.chart.update();
      })
    });
  }

  private log(data: number): void {
    console.log(data);
  }

  createChart(){
  
    this.chart = new Chart("MyChart", {
      type: 'bar', //this denotes tha type of chart

      data: {// values on X-Axis
        // labels: ['2022-05-10', '2022-05-11', '2022-05-12','2022-05-13',
				// 				 '2022-05-14', '2022-05-15', '2022-05-16','2022-05-17', ], 
         labels: [],
	       datasets: [
          {
            label: "Sales",
            // data: ['467','576', '572', '79', '92',
						// 		 '574', '573', '576'],
            data: [],
            backgroundColor: 'blue'
          }
          // },
          // {
          //   label: "Profit",
          //   data: ['542', '542', '536', '327', '17',
					// 				 '0.00', '538', '541'],
          //   backgroundColor: 'limegreen'
          // }  
        ]
      },
      options: {
        aspectRatio:2.5
      }
      
    });
  }

}
