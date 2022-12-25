import ModifyWorshipOrderForm from "./ModifyWorshipOrderForm";
import newGuid from "./services/newGuid";
import { WorshipOrder } from "./types";

export default function AddWorshipOrder() {
  const emptyWorshipOrder: WorshipOrder = {
    id: newGuid(),
    date: new Date(),
    songSets: [{
      title: 'Opening Song',
      songs: [{ songId: '' }]
    }, {
      title: 'Song Set',
      songs: [{ songId: '' }, { songId: '' }, { songId: '' }]
    }, {
      title: 'Songof Response',
      songs: [{ songId: '' }]
    }],
    tags: []
  };

  return <>
    <h1>Add Worship Order</h1>
    <ModifyWorshipOrderForm origWorshipOrder={emptyWorshipOrder} />
  </>;
}