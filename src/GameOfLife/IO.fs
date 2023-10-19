namespace GameOfLife

open System

module IO =

  let cellChar = '0'
  let delay = TimeSpan.FromMilliseconds 100
  let wrapDelta = 100

  let printAt (x, y) (c: char) = Console.Write $"\027[{y + 1};{x + 1}H{c}"

  let wrap delta (x, y) =
    0 - delta <= x
    && x < Console.WindowWidth + delta
    && 0 - delta <= y
    && y < Console.WindowHeight + delta

  let run grid =
    Console.CursorVisible <- false

    let rec loop grid =
      Console.Clear()
      grid |> Array.iter (fun (x, y) -> cellChar |> printAt (x, y))
      Threading.Thread.Sleep delay
      Grid.tick grid |> Array.filter (wrap wrapDelta) |> loop

    grid |> loop