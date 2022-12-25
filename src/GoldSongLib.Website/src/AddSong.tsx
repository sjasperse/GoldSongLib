import { MouseEvent } from "react";
import newGuid from "./services/newGuid";

export default function AddSong() {

  function onCancel(e: MouseEvent<HTMLButtonElement>) {
    e.preventDefault();

    history.back();
  }

  async function onAdd(e: MouseEvent<HTMLButtonElement>) {
    e.preventDefault();
    const nameElement = document.getElementsByName('name')[0] as HTMLInputElement;

    if (!nameElement.value) {
      return;
    }

    await fetch(`/api/songs`, {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify({
        id: newGuid(),
        name: nameElement.value
      })
    });

    history.back();
  }

  return <>
    <h1>Add Song</h1>
    <form>
      <div style={{ paddingBottom: '2em' }}>
        <label>Name</label>
        <input name="name" type="text" />
      </div>

      <button onClick={onCancel}>Cancel</button>
      <button className="primary" onClick={onAdd}>Add</button>
    </form>
  </>
}