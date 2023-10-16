open System
open GameOfLife

[<EntryPoint>]
let main args =
  let cellChar = '0'
  let delay = TimeSpan.FromMilliseconds 50
  let wrapDelta = 100
  Console.CursorVisible <- false

  let rec loop grid =
    let wrap delta (x, y) =
      0 - delta <= x
      && x < Console.WindowWidth + delta
      && 0 - delta <= y
      && y < Console.WindowHeight + delta

    Console.Clear()
    grid |> Array.filter (wrap wrapDelta) |> Array.iter (fun (x, y) -> cellChar |> printAt (x, y))
    Threading.Thread.Sleep delay
    grid |> tick |> Array.filter (wrap wrapDelta) |> loop

  (Console.WindowWidth, Console.WindowHeight) ||> random |> loop
  0
