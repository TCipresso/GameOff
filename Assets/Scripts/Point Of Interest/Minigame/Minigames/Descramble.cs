using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Descramble is a <see cref="PromptType"/> <see cref="Minigame"/> where
/// the player is presented with a scrambled word and they need 
/// to type it descrambled.
/// </summary>
public class Descramble : PromptType
{
    protected override void NewPrompt()
    {
        int prev = curPromptIndex;
        curPromptIndex = Random.Range(0, prompts.Count);
        if (curPromptIndex == prev) curPromptIndex = (curPromptIndex + 1) % prompts.Count;

        curPrompt = prompts[curPromptIndex];
        promptArea.text = ScrambleWord(curPrompt);
    }

    /// <summary>
    /// Scramble the word.
    /// </summary>
    /// <param name="original">The original word</param>
    /// <returns>The scrambled word</returns>
    protected string ScrambleWord(string original)
    {
        char[] chars = original.ToCharArray();
        int prevStart = -1;
        int prevEnd = -1;
        for (int i = 0; i < chars.Length; i++)
        {
            int start = Random.Range(0, chars.Length);
            if(start == prevStart) start = (start + 1) % chars.Length;
            prevStart = start;

            int end = Random.Range(0, chars.Length);
            if (end == prevEnd) end = (end + 1) % chars.Length;
            if(end == start) end = (end + 2) % chars.Length;
            prevEnd = end;

            char temp = chars[start];
            chars[start] = chars[end];
            chars[end] = temp;
        }

        StringBuilder result = new StringBuilder();
        for(int i = 0; i < chars.Length; i++)
        {
            result.Append(chars[i]);
        }
        return result.ToString();
    }
}
