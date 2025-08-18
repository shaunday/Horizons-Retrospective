import { Request, Response } from 'express';
import { loadElementTemplates, TemplateSet } from '../templates/templateLoader';
import { parseTradeInputWithAI } from '#services/aiClient';

let cachedTemplates: TemplateSet | null = null;

async function ensureTemplatesLoaded(): Promise<void> {
    if (!cachedTemplates) {
        const { templates, errors } = await loadElementTemplates();
        if (Object.keys(errors).length) {
            throw new Error('Failed to load templates: ' + JSON.stringify(errors));
        }
        cachedTemplates = templates;
    }
}

export async function getCreationFlows(req: Request, res: Response): Promise<void> {
    res.json({
        flows: ['new trade', 'add element to existing'],
    });
}

export async function handleCreationStep(req: Request, res: Response): Promise<Response | void> {
    try {
        await ensureTemplatesLoaded();
    } catch (err: unknown) {
        console.error(err);
        const message = err instanceof Error ? err.message : String(err);
        return res.status(500).json({ error: 'Failed to load templates', details: message });
    }

    const { flowType, userInput } = req.body as { flowType?: string; userInput?: string };

    if (!flowType || typeof flowType !== 'string') {
        return res.status(400).json({ error: 'flowType (string) is required' });
    }

    if (flowType === 'new trade') {
        const template = cachedTemplates?.tradeOrigin;
        if (!template) {
            return res.status(500).json({ error: 'Trade origin template not found' });
        }
        return res.json({ template });
    }

    if (flowType === 'add element to existing') {
        if (!userInput || typeof userInput !== 'string') {
            return res.status(400).json({ error: 'userInput string is required for this flow' });
        }

        try {
            const aiResult = await parseTradeInputWithAI(userInput);
            return res.json({ aiResult });
        } catch (err: unknown) {
            console.error('AI parsing error:', err);
            const message = err instanceof Error ? err.message : String(err);
            return res.status(500).json({ error: 'Failed to process input with AI', details: message });
        }
    }

    return res.status(400).json({ error: `Unknown flowType: ${flowType}` });
}
