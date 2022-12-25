import { Link } from "react-router-dom";

export default function WorshipOrders() {

  return <>
    <h1>Worship Orders</h1>

    <ul>
      <li><Link to="/worship-orders/add">Add new Worship Order...</Link></li>
    </ul>
  </>;
}